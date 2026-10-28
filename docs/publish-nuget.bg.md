# Публикуване на RayCraft.Blazor в nuget.org

Този документ е за Петър. Workflow-ът `.github/workflows/publish-nuget.yml` НЕ строи библиотеката: взема готовия `RayCraft.Blazor.<версия>.nupkg` от GitHub Release (същия двоичен пакет, който е в nuget.org) и го пуска в nuget.org през Trusted Publishing (OIDC). В репото няма API ключ.

## Еднократна подготовка

1. **Environment.** В GitHub: репото RayCraftUI/RayCraft > Settings > Environments > New environment, име точно `nuget-release`. Включи "Required reviewers" и добави себе си (Петър). По желание ограничи "Deployment branches" до `main`.
2. **Политика в nuget.org.** Политиката "RayCraft Free" (owner DiPiGi, repository RayCraftUI/RayCraft, workflow `publish-nuget.yml`, environment `nuget-release`, glob `RayCraft.Blazor`) вече е създадена, но е неактивна.
3. **Потребителско име.** В workflow-а `NUGET_USER` е `petergutev` (профилното име в nuget.org). Ако профилното име е друго, поправи реда `NUGET_USER` преди първото пускане.

## Преди всяко публикуване

1. Направи release в GitHub (Releases > Draft a new release) с таг, например `v1.0.0`. Качи като файл точно един `RayCraft.Blazor.1.0.0.nupkg` (без `.snupkg` и други `.nupkg`). Публикувай release-а.
2. Един-два дни преди публикуването (05-06.11) влез в nuget.org > Trusted Publishing, при политиката "RayCraft Free" натисни **Activate for 7 days**, за да е активна в деня на публикуването (07.11). Ако срокът изтече, политиката става неактивна и входът ще се провали; активирай я отново.

## Пускане

1. GitHub > Actions > "Publish RayCraft.Blazor to nuget.org" > Run workflow.
2. В полето `tag` въведи тага на release-а (например `v1.0.0`) и стартирай.
3. Задачата ще чака одобрение (Required reviewers): отвори я и натисни "Approve and deploy".
4. Workflow-ът проверява, че в release-а има точно един `.nupkg` с име и id `RayCraft.Blazor`, влиза в nuget.org през OIDC и пуска пакета с `--skip-duplicate` (ако същата версия вече съществува, нищо не се променя).

## Проверка на резултата

- В Actions всички стъпки са зелени, а стъпката "Push to nuget.org" показва `Your package was pushed.` (или предупреждение, че версията вече съществува).
- На https://www.nuget.org/packages/RayCraft.Blazor се вижда новата версия (индексирането и валидацията могат да отнемат няколко минути).
- Опитай в празен проект: `dotnet add package RayCraft.Blazor --version <версия>`.

## При грешка

- **Неуспешен вход (login failed / няма съвпадаща политика):** политиката не е активна (натисни "Activate for 7 days"), или името на репото, workflow файла, environment-а или профилното име не съвпада с политиката.
- **Не е един пакет:** поправи release-а (само един `RayCraft.Blazor.<версия>.nupkg`) и пусни отново.
- **Публикуван е грешен пакет:** версия в nuget.org не може да се изтрие. В nuget.org > Manage Packages > RayCraft.Blazor отметни версията в "Listed" и я премахни от списъка (**unlist**), така че да не се предлага на нови потребители, и публикувай нова поправена версия (с по-голям номер). Не пускай отново същата версия.
