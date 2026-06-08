# Forth Hővezetési Szimuláció
Egydimenziós hővezetési szimuláció Forth nyelven. A program egy rúd hőmérséklet-eloszlásának időbeli változását számolja explicit véges differenciás módszerrel, miközben a környezettel történő hőcsere (Newton-féle lehűlés) is figyelembe van véve.

## Modell
A program az alábbi differenciálegyenletet oldja meg:

\[
\frac{\partial T}{\partial t}
=
\alpha \frac{\partial^2 T}{\partial x^2}
-
\beta (T-T_{env})
\]

ahol:
- `T` – hőmérséklet
- `α` – hődiffuzivitás
- `β` – konvekciós hőátadási együttható
- `Tenv` – környezeti hőmérséklet

## Kezdeti feltételek
- A teljes rúd kezdetben a környezet hőmérsékletén van.
- A rúd közepén egy 500 °C-os pontszerű hőforrás található.

## Peremfeltételek
Neumann-féle (szigetelt) peremfeltételek:
\[
\frac{\partial T}{\partial x}=0
\]

A szélső pontok mindig a szomszédos pont hőmérsékletét veszik fel.

## Alapértelmezett paraméterek
| Paraméter | Érték |
|-----------|--------|
| α (ALPHA) | 1.16e-4 |
| β (BETA) | 1.74e-3 |
| Tenv | 20 °C |
| NODES | 61 |
| TOTAL-TIME | 10 s |
| OUT-STEP | 0.5 s |

## Fájlok
- `szim.fs` – a szimuláció forráskódja

## Futtatás Gforth alatt
### Ubuntu / Linux
```bash
gforth szim.fs
```

## Fontos változók
### Rácsfelbontás
```forth
61 NODES !
```

### Szimulációs idő
```forth
20e TOTAL-TIME f!
```

### Kimeneti időlépés
```forth
1e OUT-STEP f!
```

### ASCII megjelenítés
Bekapcsolva:
```forth
1 DRAW-ASCII? !
```
Kikapcsolva:
```forth
0 DRAW-ASCII? !
```

## Futtatás
```forth
simulate
```
## Kimenet
### ASCII mód
A hőmérséklet-eloszlás oszlopdiagramként jelenik meg:
```text
=== IDO: 2.0 s ===

        ##
      ######
    ##########
...
```

### Numerikus mód
```text
Ido: 2.0

0 20
1 20
2 20.1
...
30 412.5
...
```

## Numerikus módszer

A program:
1. Explicit véges differenciás sémát használ.
2. Automatikusan kiszámít egy stabil időlépést.
3. Egy kimeneti időlépésen belül több belső integrációs lépést hajt végre (`SUB-STEPS`).
A stabilitási feltétel:
\[
\Delta t \le \frac{\Delta x^2}{4\alpha}
\]

## Adatszerkezet
Két tömb tárolja a hőmérsékleteket:

- `Told` – aktuális állapot
- `Tnew` – következő időlépés

Maximum:
```forth
MAX-NODES = 2000
```

## Példa
```forth
101 NODES !
30e TOTAL-TIME f!
0.25e OUT-STEP f!

simulate
```

## Licenc
Szabadon felhasználható oktatási és demonstrációs célokra.
