import { readFileSync, readdirSync, existsSync } from 'node:fs';
import { join } from 'node:path';

const stringPattern = String.raw`"((?:[^"\\]|\\.)*)"`;
const safeCommon = new Set(['Class', 'Style', 'ChildContent', 'Disabled', 'Placeholder', 'HelperText', 'Value', 'ValueChanged', 'ReadOnly', 'Invalid', 'DisplayName', 'Min', 'Max']);

const decode = (text, rename, others) => {
  const sentences = rename(JSON.parse(`"${text}"`)).split(/(?<=\.)\s+/);
  const kept = sentences.filter((sentence) => !others.some((name) => new RegExp(String.raw`\b${name}\b`).test(sentence)));
  return kept.join(' ');
};

export function collectWrittenDocs(clientDir, freeNames, rename) {
  const common = {};
  const leads = {};
  const parameters = {};
  const others = ['Windows', 'macOS'];
  const pagesList = join(clientDir, 'Pages', 'Components');
  if (existsSync(pagesList)) {
    for (const file of readdirSync(pagesList)) {
      const name = file.replace(/Page\.razor$/, '');
      if (file.endsWith('Page.razor') && !freeNames.has(name)) others.push(name);
    }
  }

  const apiTable = join(clientDir, 'Docs', 'ApiTable.razor.cs');
  if (existsSync(apiTable)) {
    const text = readFileSync(apiTable, 'utf8');
    const entry = new RegExp(String.raw`\["(\w+)"\]\s*=\s*${stringPattern}`, 'g');
    for (const match of text.matchAll(entry)) {
      if (safeCommon.has(match[1]) && common[match[1]] === undefined) common[match[1]] = decode(match[2], rename, others);
    }
  }

  const pagesDir = join(clientDir, 'Pages', 'Components');
  if (existsSync(pagesDir)) {
    const lead = new RegExp(String.raw`<DocPage Title="(\w+)" Lead="@T\(${stringPattern}`, 'g');
    const table = /<ApiTable Component="typeof\((\w+)[^"]*\)"([\s\S]*?)\/>/g;
    const entry = new RegExp(String.raw`\["(\w+)"\]\s*=\s*(?:T\()?${stringPattern}`, 'g');
    for (const file of readdirSync(pagesDir).filter((f) => f.endsWith('.razor')).sort()) {
      const text = readFileSync(join(pagesDir, file), 'utf8');
      for (const match of text.matchAll(lead)) {
        if (freeNames.has(match[1])) leads[match[1]] = decode(match[2], rename, others);
      }
      for (const tag of text.matchAll(table)) {
        if (!freeNames.has(tag[1])) continue;
        parameters[tag[1]] ??= {};
        for (const item of tag[2].matchAll(entry)) parameters[tag[1]][item[1]] = decode(item[2], rename, others);
      }
    }
  }

  return { common, leads, parameters };
}

const quote = (text) => JSON.stringify(text);
const pairs = (map) => Object.entries(map).map(([k, v]) => `[${quote(k)}] = ${quote(v)}`).join(', ');

export function writtenDocsSource({ common, leads, parameters }) {
  const lines = ['namespace RayCraft.Examples.Docs;', '', 'public static class WrittenDocs', '{'];
  lines.push(`    public static IReadOnlyDictionary<string, string> Common { get; } = new Dictionary<string, string> { ${pairs(common)} };`);
  lines.push('', `    public static IReadOnlyDictionary<string, string> Leads { get; } = new Dictionary<string, string> { ${pairs(leads)} };`);
  lines.push('', '    public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Parameters { get; } = new Dictionary<string, IReadOnlyDictionary<string, string>>', '    {');
  for (const [name, map] of Object.entries(parameters)) lines.push(`        [${quote(name)}] = new Dictionary<string, string> { ${pairs(map)} },`);
  lines.push('    };', '}', '');
  return lines.join('\n');
}
