import { readFileSync, writeFileSync, readdirSync, mkdirSync, existsSync } from 'node:fs';
import { join, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { collectWrittenDocs, writtenDocsSource } from './written-docs.mjs';

const root = join(dirname(fileURLToPath(import.meta.url)), '..');
const clientDir = process.argv[2];
const sourceName = process.argv[3];
if (!clientDir || !sourceName || !existsSync(join(clientDir, 'Examples'))) {
  console.error('usage: node tools/generate-examples.mjs <source demo folder with Examples, Docs and Pages/Components> <product name used in the source>');
  process.exit(1);
}
const sourceDir = join(clientDir, 'Examples');

const targetDir = join(root, 'src', 'RayCraft.Examples', 'Examples');
const catalogFile = join(root, 'src', 'RayCraft.Examples', 'Docs', 'ExampleIndex.g.cs');
const free = JSON.parse(readFileSync(join(root, 'tools', 'free-controls.json'), 'utf8'));
const excluded = JSON.parse(readFileSync(join(root, 'tools', 'excluded-examples.json'), 'utf8'));
const manual = JSON.parse(readFileSync(join(root, 'tools', 'manual-examples.json'), 'utf8'));

const aliases = {
  ProgressBasic: ['ProgressBar'],
  SnackbarBasic: ['SnackbarHost'],
  ThemeAndLanguage: ['ThemeToggle', 'DensityToggle', 'LanguageToggle'],
  BreakpointUtilities: ['BreakpointObserver'],
};

const rules = [
  [new RegExp(sourceName, 'g'), 'RayCraft'],
  [new RegExp(sourceName.toLowerCase(), 'g'), 'raycraft'],
  [new RegExp(sourceName.toUpperCase(), 'g'), 'RAYCRAFT'],
];

const rename = (text) => rules.reduce((value, [pattern, replacement]) => value.replace(pattern, replacement), text)
  .replace(/RayCraft\.Demo\.Client\.Examples/g, 'RayCraft.Examples.Examples');

const categoryOf = new Map();
for (const [category, names] of Object.entries(free)) for (const name of names) categoryOf.set(name, category);
const names = [...categoryOf.keys()].sort((a, b) => b.length - a.length);

const controlOf = (file) => {
  if (aliases[file]) return aliases[file];
  const match = names.find((n) => file.startsWith(n) && (file.length === n.length || /[A-Z]/.test(file[n.length])));
  return match ? [match] : [];
};

const titleOf = (file, control) => {
  const rest = file.slice(control.length);
  if (rest === '') return 'Basic';
  const words = rest.replace(/([a-z0-9])([A-Z])/g, '$1 $2').toLowerCase();
  return words[0].toUpperCase() + words.slice(1);
};

mkdirSync(targetDir, { recursive: true });
const sampleData = join(sourceDir, 'SampleData.cs');
if (existsSync(sampleData)) writeFileSync(join(targetDir, 'SampleData.cs'), rename(readFileSync(sampleData, 'utf8')));
const entries = new Map(names.map((n) => [n, []]));
const skipped = [];

for (const file of readdirSync(sourceDir).filter((f) => f.endsWith('.razor')).sort()) {
  const base = file.slice(0, -'.razor'.length);
  const controls = controlOf(base);
  if (controls.length === 0) {
    skipped.push([base, 'no free control']);
    continue;
  }
  if (excluded[base]) {
    skipped.push([base, excluded[base]]);
    continue;
  }
  if (!manual.includes(base)) {
    writeFileSync(join(targetDir, file), rename(readFileSync(join(sourceDir, file), 'utf8')));
  }
  for (const control of controls) entries.get(control).push([base, titleOf(base, control)]);
}

for (const base of manual) {
  const control = controlOf(base)[0];
  if (!entries.get(control).some(([b]) => b === base)) entries.get(control).push([base, titleOf(base, control)]);
}

const used = new Map();
for (const file of readdirSync(targetDir).filter((f) => f.endsWith('.razor'))) {
  const text = readFileSync(join(targetDir, file), 'utf8');
  for (const name of names) {
    if (new RegExp(String.raw`<${name}[\s/>]`).test(text)) {
      if (!used.has(name)) used.set(name, []);
      used.get(name).push(file.slice(0, -'.razor'.length));
    }
  }
}

const quote = (text) => JSON.stringify(text);
const lines = [];
lines.push('namespace RayCraft.Examples.Docs;', '', 'public static class ExampleIndex', '{');
lines.push('    public static IReadOnlyList<IndexEntry> Entries { get; } =', '    [');
for (const [category, list] of Object.entries(free)) {
  for (const name of list) {
    const own = entries.get(name).map(([base, title]) => `new(${quote(title)}, typeof(Examples.${base}))`);
    const owners = new Set(entries.get(name).map(([base]) => base));
    const borrowed = own.length > 0 ? [] : (used.get(name) ?? []).filter((b) => !owners.has(b)).slice(0, 2)
      .map((b) => `new(${quote('In use: ' + b.replace(/([a-z0-9])([A-Z])/g, '$1 $2'))}, typeof(Examples.${b}))`);
    lines.push(`        new(${quote(category)}, ${quote(name)}, [${[...own, ...borrowed].join(', ')}]),`);
  }
}
lines.push('    ];', '}', '');
writeFileSync(catalogFile, lines.join('\n'));

const covered = [...entries.values()].filter((e) => e.length > 0).length;
console.log(`examples written: ${[...entries.values()].reduce((s, e) => s + e.length, 0)}, controls with own examples: ${covered} of ${names.length}`);
console.log(`skipped: ${skipped.length}`);

const written = collectWrittenDocs(clientDir, new Set(names), rename);
writeFileSync(join(root, 'src', 'RayCraft.Examples', 'Docs', 'WrittenDocs.g.cs'), writtenDocsSource(written));
console.log(`leads: ${Object.keys(written.leads).length}, controls with parameter descriptions: ${Object.keys(written.parameters).length}, parameter descriptions: ${Object.values(written.parameters).reduce((n, m) => n + Object.keys(m).length, 0)}, common: ${Object.keys(written.common).length}`);
