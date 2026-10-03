<div align="center">

# Make Shotguns Great Again!

Shotguns do jeito certo no SPT 4.1.6: shotguns do jogo melhoradas, armas, acessórios e munições novas, faíscas de Dragon's Breath e uma reformulação das panes de arma.

![Version](https://img.shields.io/badge/version-1.16.0-orange?style=flat)
![SPT](https://img.shields.io/badge/SPT-4.1.6-blue?style=flat)
![WTT-CommonLib](https://img.shields.io/badge/WTT--CommonLib-3.0.6-purple?style=flat)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=flat&logo=dotnet)
![License](https://img.shields.io/badge/license-MIT-green?style=flat)

[Funcionalidades](#funcionalidades) · [Instalação](#instalação) · [Armas novas](#armas-novas) · [Munições novas](#munições-novas) · [Configuração](#configuração) · [Build](#build-a-partir-do-código)

[English](README.md) · **Português**

![fire](https://media1.giphy.com/media/v1.Y2lkPTc5MGI3NjExcTZiZzdxeXJ0eTBxNWNtbHVxdzBweWkxbjhiNnJoc2ZiOTh4a3VzZyZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/u1aUNE2xRmk3xUaSAJ/giphy.gif)

</div>

---

## Funcionalidades

**Armas do jogo**

- **Benelli M3**: modo semiautomático ajustado para ficar igual às outras semiautomáticas (MP-153, MP-155).
- **Saiga-12K**: aceita a maioria dos handguards de AK.
- **KS-23M**: aceita o suporte de trilho Kiba Arms SPRM e o recoil pad do AK GP-25.
- **MTs-255**: aceita os suportes de trilho ETMI-019 e Kiba Arms SPRM.
- **Mossberg 590A1**: aceita o suporte de trilho Kiba Arms SPRM e um [cano rosqueado](#acessórios-novos) novo, para choke e supressor.
- **MP-18 (7.62x54R)**: aceita um [cano rosqueado](#acessórios-novos) novo, para freio de boca e supressor.
- **ETMI-019 e Kiba Arms SPRM**: aceitam mais miras e suportes.
- **AA-12**: cadência de tiro maior.
- **Slug Barrikada**: precisão aumentada para ficar igual aos outros slugs 12ga.

**Client**

- **Limpar sem inspecionar**: resolve a pane na hora com a tecla de limpar, sem inspecionar a arma antes.
- **Sem pane forçada por chefes**: chefes como o Kollontay não conseguem forçar pane na sua arma. Panes só vêm da condição da arma e da munição.
- **Efeito de Dragon's Breath**: atirar 12/70 'Hellfire' solta faíscas incendiárias no cano.
- **Correção das miras da KS-23**: miras no trilho da KS-23 agora acertam onde o retículo aponta.
- **Som da MP-18 com supressor**: a MP-18 com supressor agora soa abafada. O jogo tinha o som no lugar errado da arma.
- **Chumbo em qualquer arma**: chumbo disparado de armas que não são shotgun (como a 5.45x39 Svalka num AK) espalha os balins em vez de acertar um ponto só.

**Conteúdo novo**

- 4 [armas](#armas-novas), 9 [acessórios](#acessórios-novos) e 11 [itens de munição](#munições-novas), vendidos pelos traders e fabricáveis na Workbench.
- Scavs podem aparecer com a MP-12 e a MP-700 e usar munição FRAG-12, Hellfire e .700 Nitro. O Tagilla pode usar Hellfire.

---

## Instalação

Instale antes o [WTT-CommonLib](https://github.com/GrooveypenguinX/WTT-CommonLib) **3.0.6** ou mais recente (3.0.x). Depois extraia o `makeshotgunsgreatagain.zip` na pasta do jogo SPT:

```
<pasta do jogo>/
├── BepInEx/plugins/makeshotgunsgreatagain.dll
└── SPT_Runtime/user/mods/makeshotgunsgreatagain/
    ├── makeshotgunsgreatagain.dll
    ├── bundles.json
    ├── bundles/            modelos dos itens novos
    ├── config/config.json
    └── db/                 itens, presets, ofertas dos traders, crafts e locales
```

As duas partes são necessárias: o server adiciona os itens e o client cuida dos efeitos e correções.

> Os itens novos ficam no seu perfil. Remover o mod depois de comprar ou lootear algum deles quebra o perfil.

---

## Armas novas

| Arma | Calibre | Vendida por |
|---|---|---|
| MP-12 12g single-shot rifle | 12/70 | O Jaeger LL1 vende as versões montadas **wood** (6.875 ₽) e **polymer** (7.103 ₽) |
| MP-700 .700 Nitro Express Double Rifle | .700 Nitro Express | Jaeger LL3 (129.564 ₽). Montada no Jaeger LL2 (195.632 ₽) |
| MP-700 Shorty .700 Nitro Express Double Rifle | .700 Nitro Express | Jaeger LL3 (145.000 ₽). Montada no Jaeger LL3 (215.746 ₽) |
| ZiD SP-81 23x75 pistol | 23x75 | Mechanic LL2 (65.321 ₽). Pistola de sinalização refeita para 23x75; não dispara sinalizador |

A MP-700 Shorty é uma MP-700 serrada: sem coronha, cano curto e recuo brutal.

O Mechanic LL2 também vende a versão **MP-18 Tactical**: uma MP-18 com o cano rosqueado, supressor SIG Sauer SRD762Ti e luneta Burris FullField TAC30 1-4x24, por 4 Weapon parts e 2 Gunpowder "Eagle".

### Acessórios novos

| Acessório | Vendido por |
|---|---|
| Mossberg 590A1 12ga 508mm threaded barrel | Mechanic LL2 (15.500 ₽) |
| KS-23M 23x75 6-shell magazine | Mechanic LL2 (4.600 ₽) |
| Benelli M3 Keymod Handguard | Mechanic LL2 (8.013 ₽) |
| MP-153 12ga competition 13-shell magazine | Mechanic LL3 (6.003 ₽) |
| SOK-12 12/76 Alliance Armament 30-round magazine | Mechanic LL3 (27.996 ₽) |
| MP-12 12ga 600mm barrel | Jaeger LL1 (2.134 ₽) |
| MP-18 7.62x54R 600mm threaded barrel | Mechanic LL2 (12.500 ₽) |
| MP-700 .700 Nitro Express Double Rifle 725mm Barrel | Jaeger LL3 (29.568 ₽) |
| MP-700 Shorty .700 Nitro Express 310mm Barrel | Jaeger LL3 (31.000 ₽) |

O cano rosqueado da 590A1 aceita os mesmos muzzle devices da MP-153.

O cano rosqueado da MP-18 pesa 1,45 kg (ergonomia -15) e aceita os muzzle devices do SV-98 (o thread adapter leva o supressor do SV-98) e os muzzle devices e supressores 7.62x51 de rosca direta do AR-10, SR-25 e SCAR-H.

---

## Munições novas

| Munição | Vendida por | Workbench |
|---|---|---|
| 12/70 Magnum Express Kinghunter | Peacekeeper LL2 ($2) | Nível 2 |
| 12/70 Flechette Kinghunter | Peacekeeper LL3 ($3). Caixa com 25 no LL2 por um Portable Powerbank | Nível 2 |
| 12/70 armor-piercing Slug "SVAROG" | Peacekeeper LL3 ($5). Caixa com 5 no LL2 por um Diary | Nível 3 |
| 12/70 'FRAG-12' | Peacekeeper LL4 ($106) | Nível 3 |
| 12/70 'Hellfire' hybrid buckshot | Jaeger LL2 (1.236 ₽) | Nível 3 |
| 12/70 Winchester Super-X 00 buckshot | Jaeger LL2 (76 ₽) | Nível 2 |
| 12/70 7mm Buckshot Brass Case | Jaeger LL2 (44 ₽) | Nível 1 |
| .700 Nitro Express FMJ | Jaeger LL3 (1.930 ₽) | Nível 3 |
| 5.45x39mm 'Svalka' Anti-Drone Buckshot | — | Nível 1 |

### Crafts da Workbench

| Produto | Qtd. | Nível | Tempo | Ingredientes | Ferramentas |
|---|---|---|---|---|---|
| 5.45x39mm 'Svalka' | 30 | 1 | 1 h | 30x 5.45x39mm SP, 5x 12/70 8.5mm Magnum buckshot, 1x Disposable syringe | Leatherman Multitool |
| 12/70 7mm Buckshot Brass Case | 60 | 1 | 2 h | 1x Horse figurine, 1x Gunpowder "Kite" | Pliers Elite |
| 12/70 Magnum Express Kinghunter | 50 | 2 | 2 h 20 min | 2x Geiger-Muller counter, 1x Gunpowder "Hawk" | Pliers |
| 12/70 Flechette Kinghunter | 40 | 2 | 3 h | 4x Pack of nails, 1x Gunpowder "Eagle", 1x Gunpowder "Kite", 40x 12/70 7mm buckshot | Round pliers, Flat screwdriver |
| 12/70 Winchester Super-X 00 buckshot | 40 | 2 | 1 h | 1x Gunpowder "Eagle", 2x D Size battery | Pliers |
| 12/70 armor-piercing Slug "SVAROG" | 35 | 3 | 3 h | 1x Gunpowder "Eagle", 3x Spark plug | Screwdriver |
| 12/70 'Hellfire' hybrid buckshot | 40 | 3 | 4 h | 1x Gunpowder "Hawk", 1x Can of thermite, 1x Classic matches, 1x Hunting matches | Leatherman Multitool |
| 12/70 'FRAG-12' | 15 | 3 | 5 h 30 min | 1x Gunpowder "Eagle", 2x 40mm VOG-25 grenade, 1x Metal spare parts | Pliers, Screwdriver |
| .700 Nitro Express FMJ | 20 | 3 | 5 h | 3x Gunpowder "Eagle", 1x Weapon parts, 1x Military cable | Pliers |

---

## Configuração

### Client (menu `F12`)

| Seção | Opção | Padrão | Descrição |
|---|---|---|---|
| Malfunctions | Skip Inspection Before Clearing | `true` | Limpa panes sem inspecionar a arma antes |
| Malfunctions | Remove Boss Forced Malfunctions | `true` | Impede chefes de forçar pane na sua arma |
| Dragon Breath | Trails Enabled | `true` | Rastros de fogo atrás das faíscas |
| Dragon Breath | Collision Enabled | `true` | Faíscas ricocheteiam nas paredes |
| Dragon Breath | Lights Enabled | `true` | Faíscas iluminam o ambiente |
| Dragon Breath | Max Particles | `400` | Faíscas ativas ao mesmo tempo (50–800) |
| Dragon Breath | Particles Per Shot | `100` | Faíscas por tiro (20–300) |
| Dragon Breath | Effect Duration | `4` | Segundos até o efeito sumir (1–8) |
| Dragon Breath | Spread Angle | `4` | Abertura do cone de faíscas, em graus (1–45) |
| Dragon Breath | Noise Strength | `6` | Quanto as faíscas rodopiam (0–10) |
| Dragon Breath | Noise Frequency | `5` | Velocidade do rodopio (0–10) |
| Dragon Breath | Air Resistance | `0.3` | Quão rápido as faíscas desaceleram (0–1) |
| Dragon Breath | Start Offset | `0.2` | Distância do cano onde o efeito começa, em metros (0–1) |
| KS-23 Mount Calibration | Enable Mount Alignment Fix | `true` | Corrige os tiros das miras no trilho da KS-23 |
| KS-23 Mount Calibration | Shot Pitch Correction | `0.311` | Correção vertical, em graus |
| KS-23 Mount Calibration | Shot Yaw Correction | `0.31` | Correção horizontal, em graus |
| KS-23 Mount Calibration | Debug Logging | `false` | Mostra cada correção no console do BepInEx |

Desligar rastros, colisão ou luzes, ou baixar a quantidade de partículas, deixa o efeito de Dragon's Breath mais leve.

### Server (`config/config.json`)

O arquivo fica em `SPT_Runtime/user/mods/makeshotgunsgreatagain/config/`. Reinicie o server depois de editar.

| Campo | Padrão | Descrição |
|---|---|---|
| `enableBotsUseFrag12` | `true` | Bots podem usar munição FRAG-12 |
| `enableBotsUseDragonBreath` | `true` | Bots podem usar munição Hellfire (Dragon's Breath) |
| `enableDebugLogs` | `false` | Mostra as alterações do mod no console do server |

---

## Build a partir do código

**Requisitos:** .NET 10 SDK e uma instalação do SPT 4.1.6 (o projeto do client referencia as DLLs do jogo).

```sh
dotnet build makeshotgunsgreatagain.sln -c Release
```

O projeto do server compila o client antes e, em Release, gera o `makeshotgunsgreatagain.zip` na pasta da solution, com as duas DLLs, `config/`, `db/`, `bundles/` e `bundles.json`.

> O `.csproj` do client aponta para `D:\Jogos\SPT4.1` nas referências (mude com `-p:SptGameDir=...`), e os dois projetos copiam o build para essa instalação para teste. Troque também o `SptModsDir` pela sua pasta do SPT. Feche o server do SPT antes de compilar, senão a cópia falha porque a DLL do server está em uso.

### Estrutura do projeto

```
Make-Shotguns-Great-Again/
├── makeshotgunsgreatagain.sln
├── Server/                             server mod .NET 10
│   ├── makeshotgunsgreatagain.cs       metadata, carga via WTT e alterações em itens do jogo
│   ├── ModConfig.cs                    modelo do config.json
│   ├── config/config.json
│   ├── bundles.json                    lista de bundles dos itens novos
│   ├── bundles/                        modelos dos itens novos
│   └── db/
│       ├── CustomItems/                armas, acessórios e munições novas
│       ├── CustomAssortSchemes/        trocas das caixas de munição no Peacekeeper
│       ├── CustomHideoutRecipes/       crafts da Workbench
│       ├── CustomLocales/              nomes das versões montadas
│       └── weaponPresets/
│           ├── Assorts/                armas montadas vendidas pelo Jaeger e pelo Mechanic
│           ├── BotLoadouts/            armas e munições novas para os bots
│           └── GlobalPresets/          versões da MP-12 e MP-18 Tactical
└── Client/                             plugin BepInEx (netstandard2.1)
    ├── makeshotgunsgreatagain.cs       opções do F12
    └── Patches/
        ├── BuckshotDispersionPatch.cs                     espalhamento de chumbo fora de shotguns
        ├── CanResolveMalfunctionsWithoutInspectionPatch.cs
        ├── DragonBreathMuzzlePatch.cs                     faíscas do Dragon's Breath
        ├── DragonBreathPatch.cs
        ├── KS23MountAlignmentPatch.cs                     correção das miras da KS-23
        ├── MP18SilencedSoundPatch.cs                      som da MP-18 com supressor
        └── RemoveBossMalfunctionsPatch.cs
```

---

## Créditos

- Modelo da 12/70 Winchester Super-X 00 buckshot: [Guy in a Poncho](https://sketchfab.com/ponchoguy)
- Modelos da 12/70 AP Slug SVAROG, Flechette Kinghunter e Magnum Express Kinghunter: [Deadcomrade](https://sketchfab.com/deadcomrade)
- Efeito de partículas do Dragon's Breath: baseado no trabalho de **jankytheclown** no [HollywoodFX](https://github.com/SleepingPills/HollywoodFX)

---

## Recursos

| Recurso | URL |
|---|---|
| WTT-CommonLib | https://github.com/GrooveypenguinX/WTT-CommonLib |
| SPT Server C# | https://github.com/SP-Tushonka/server-csharp |
| Exemplos de server mod | https://github.com/SP-Tushonka/server-mod-examples |
| SPT Wiki — Modding Resources | https://wiki.sp-tushonka.com/en/modding/Modding_Resources |
| SPT Scaffold | https://github.com/viniHNS/spt-scaffold |
