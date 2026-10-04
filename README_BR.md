<div align="center">

# Make Shotguns Great Again!

Shotguns do jeito certo no SPT 4.1.6: shotguns do jogo melhoradas, armas, acessórios e munições novas, faíscas de Dragon's Breath e uma reformulação das panes de arma.

![Version](https://img.shields.io/badge/version-1.17.0-orange?style=flat)
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

- 5 [armas](#armas-novas), 15 [acessórios](#acessórios-novos) e 14 [itens de munição](#munições-novas), disponíveis nos traders e em receitas da Workbench.
- Seis quests: três serviços do Skier desbloqueiam as configurações da VR80, e Hunter's Dream do Jaeger desbloqueia as novas cargas .700 Nitro.
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
| Escopeta semi-automática Rock Island Armory VR80 12ga | 12/70 | Troca no Skier LL1 após Mercadoria de fora; compra no LL2 (110.000 ₽) após Cliente habitual. Configuração de fábrica com carregador de 5 cartuchos |

A MP-700 Shorty é uma MP-700 serrada: sem coronha, cano curto e recuo brutal.

A VR80 é uma escopeta semiautomática operada a gás, com comandos no estilo AR, cano de 508 mm e carregadores destacáveis de 5 e 10 cartuchos. O guarda-mão M-LOK de fábrica aceita punhos e suportes de acessórios compatíveis; o trilho superior aceita miras. A coronha original inclui o punho. A configuração de fábrica pesa 3,35 kg com o carregador de 5 vazio.

Após **Mercadoria de fora**, o Skier LL1 troca a VR80 de fábrica por um whiskey Dan Jackiel, uma vodka Tarkovskaya e dois maços de Wilston (uma arma por reposição). Complete **Cliente habitual** no Skier LL2 para liberar a compra por 110.000 ₽.

O Skier LL2 também troca a versão **VR80 Silenciosa** após a quest independente **Acordo discreto** (uma por reposição): cano rosqueado com supressor SilencerCo Salvo 12, mira EOTech XPS3-2, empunhadura Magpul AFG e carregador de 10, por um Roler Submariner, um relógio de madeira, uma corrente de ouro e uma estatueta de cavalo.

| Quest do Skier | Requisitos e objetivos | Desbloqueio |
| --- | --- | --- |
| Mercadoria de fora | LL1; elimine 5 Scavs na Customs com qualquer escopeta calibre 12 e entregue 2 Wilston encontrados em raid | Troca da VR80 de fábrica no LL1 |
| Cliente habitual | LL2 e Mercadoria de fora concluída; elimine 10 Scavs na Customs com a VR80 | Compra da VR80 de fábrica por dinheiro no LL2 |
| Acordo discreto | LL2, independente das outras duas; elimine 8 Scavs na Customs e 3 PMCs em qualquer mapa com qualquer escopeta calibre 12 com supressor | Troca da VR80 Silenciosa no LL2 |

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
| VR80 12ga 5-round magazine | Skier LL2 (6.000 ₽) |
| VR80 12ga 10-round magazine | Skier LL2 (9.000 ₽) |
| VR80 stock with pistol grip | Skier LL2 (8.000 ₽) |
| VR80 12ga 508mm barrel | Skier LL2 (14.000 ₽) |
| VR80 12ga 508mm threaded barrel | Mechanic LL2 (15.500 ₽) |
| VR80 M-LOK handguard | Skier LL2 (11.000 ₽) |

O cano rosqueado da 590A1 aceita os mesmos muzzle devices da MP-153.

O carregador Alliance Armament de 30 cartuchos da Saiga recebeu UVs novos e texturas de alumínio anodizado com desgaste.

O cano rosqueado da VR80 pesa 0,9 kg (ergonomia -10) e aceita os mesmos muzzle devices da MP-153.

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
| .700 Nitro Express SP "Mammoth" | Jaeger LL3 (2.800 ₽), após Hunter's Dream - Part 1 | — |
| .700 Nitro Express AP "Goliath" | Jaeger LL4 (8.000 ₽), após Hunter's Dream - Part 3 | — |
| .700 Nitro Express chumbo grosso "Cerberus" | Jaeger LL3 (3.600 ₽), após Hunter's Dream - Part 2 | — |
| 5.45x39mm 'Svalka' Anti-Drone Buckshot | — | Nível 1 |

A MP-700 e a MP-700 Shorty também aceitam três cargas especiais .700 Nitro Express: **Mammoth** expansiva (430 de dano, 15 de penetração), **Goliath** perfurante (220 de dano, 60 de penetração) e **Cerberus** de chumbo grosso (8 projéteis com 65 de dano e 8 de penetração cada). São variantes criadas para o mod, incluindo cargas experimentais ficcionais. A FMJ não atravessa placas, mas acaba com a durabilidade delas. Cada uma tem uma ponta e marcas de identificação próprias.

### Hunter's Dream

A questline de três partes do Jaeger começa no nível 30, após **Acquaintance**. Cada parte exige a conclusão da anterior. Use a **MP-700** ou a **MP-700 Shorty**, com qualquer munição compatível; o progresso acumula entre raids.

| Quest | Objetivo | Compra desbloqueada |
|---|---|---|
| Hunter's Dream - Part 1 | Eliminar 15 Scavs em Woods a pelo menos 40 metros | Mammoth SP, Jaeger LL3 |
| Hunter's Dream - Part 2 | Eliminar 8 PMCs a no máximo 30 metros, em qualquer mapa | Cerberus, Jaeger LL3 |
| Hunter's Dream - Part 3 | Eliminar Tagilla na Factory, de dia ou de noite | Goliath AP, Jaeger LL4 |

Concluir uma quest desbloqueia sua oferta de compra; o nível de lealdade do trader continua obrigatório. A compra da Goliath exige LL4 (nível de jogador mínimo 33), mesmo concluindo a Part 3 antes disso. Cada quest também entrega experiência, rublos, reputação com o Jaeger e uma pequena quantidade da munição desbloqueada. A compra e o craft da FMJ publicada continuam disponíveis como antes.

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
│       ├── Quests/                     quests da VR80 no Skier e Hunter's Dream
│       └── weaponPresets/
│           ├── Assorts/                armas montadas vendidas pelo Jaeger, Mechanic e Skier
│           ├── BotLoadouts/            armas e munições novas para os bots
│           └── GlobalPresets/          configurações padrão das armas
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

- Escopeta Rock Island Armory VR80 e suas peças: adaptadas do [modelo de White-Horse](https://skfb.ly/6WP7G), licenciado sob [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/). O modelo foi separado em peças modulares e adaptado aos materiais e ao esqueleto de arma do jogo. Esses assets mantêm a atribuição CC BY 4.0; a licença MIT do mod cobre o código.
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
