# Prvý dojem
Knižnica ArgParser na prvý dojem pôsobí kvalitne a je vidieť, že si autori dali
záležať na jej návrhu. Hlavnými črtmi už teraz pôsobí tak, že podporuje parsovanie 
argumentov pre veľkú škálu command-line programov s rozličnými typmi argumentov
a ich parametrami.
Repozitár knižnice je dobre štruktúrovaný a obsahuje aj niekoľko užitočných 
ukážkových aplikácií, ktoré zjednodušujú jej prvotné použitie. Taktiež nechýba
ani základná dokumentácia k správnemu použitiu a inštrukcie k integrácii knižnice 
do existujúcich projektov.

# API review
Knižnica prenáša na seba svojim API veľkú časť zodpovednosti čo sa týka 
zadefinovania argumentov, kontroly ich správnosti a taktiež uloženia naparsovaných hodnôt.
Týmto užívatelovi celkovo spríjemnuje jej použitie k parsovaniu argumentov v jeho programe.
Uživatel sa nemusí starať o typovú kontrolu a k hodnotám argumentov sa vie jednoducho dostať
dostať cez objekt parsera.

Samotné zadefinovanie parsera a jeho použitie zabraňuje zbytočným chýbam 
už pri kompilácii programu vďaka silne typovanému prístupu k hodnotám argumentov.
API knižnice taktiež rieši aj chybové stavy a to tak, že jednotlivé metódy vyhadzujú správne
výnimky, čo dáva užívatelovi možnosť na tieto chybové stavy nejakým spôsobom reagovať.

Dokumentácia API je z veľkej časti dostatočná, no nájdu sa niektoré časti API, kde nie je 
úplne zrejmé, čo daná vec robí.

# numactl
Použitie knižnice výrazne zjednodušilo a zrýchlilo implementáciu
parsovania argumentov pre program numactl. 
Zadefinovanie argumentov programu ako členov triedy dediacej z `ParserBase` 
je praktické a vnieslo do kódu prehladnosť podporovaných argumentov. Taktiež to
umožnilo veľmi jednoduchú prácu s jednotlivými argumentami a ich naparsovanými 
hodnotami.

Táto implementácia však odhalila aj niekoľko nedostatkov. Parsovania argumentu `args`, ktorý 
má obsahovať argumenty pre spustenie príkazu zadaného v argumente `command`, sa nedalo
vôbec naimplementovať. Ak knižnica toto nejakým spôsobom nakoniec podporuje, nie je 
to vôbec zrejmé z jej dokumentácie. Ďalej pri definicii argumentov sa nedal zadať názov pre
jednotlivé parametre argumentov - napríklad pri možnosti `preferred` je názov parametru `<node>`.
A ako posledná vec, ktorá mierne znepríjemnila implementáciu v numactl, sú vzájomne vylučujúce sa
options `-m, -p, -i`. Knižnica s týmto nijako nepomohla a skontrolovanie výlučnosti tak bolo ponechané 
na užívateľa knižnice.

# Doplňujúci komentár
Nedostatky:
- Nemožnosť získania počtu naparsovaných parametrov pre prípady, kedy je počet parametrov ľubovolný. 
Taktiež sa nedá ani získať všetky naparsované parametre naraz ako jednu kolekciu.
- Chýba nastavitelnosť vzájmne vylučujúcich sa argumentov.
- Chýba nastavitelnosť názvov pre jednotlivé parametre v argumentoch.
- Jednotlivé triedy by možno bolo vhodné rozdeliť do samostatných súborov. Teraz je mnoho tried nakope 
a celkom ťažko sa orientuje v kóde.
- Nie je úplne jasné z dokumentácie rozdelenie medzi Argument a Option. Tieto triedy vyzerajú veľmi
podobne - nedá sa to nejako zjednotiť v prípade, že sa tak veľmi od seba nelíšia?

