# Tron Race
## komponenty
### Hierarchie
Godot je postaven na pomocí [inheretence ](https://docs.godotengine.org/en/latest/contributing/development/core_and_modules/inheritance_class_tree.html) a na uzpůsoben na [kompozici](https://docs.godotengine.org/en/latest/getting_started/introduction/godot_design_philosophy.html), proto jsme vytvořili zpousta komponentů díky kterým můžeme rychle přidávat nové věci do hry.
Příklad  *louncher ship*, která je vytvořená z [health_component](###health_component), [move_component](###move_component), *missale_louncher* -> který je zložený z rotate_component, gun_component (který mohl kdyby byl vytvořený pozděj ještě být vytvořen ze spawn_componentu.
Tento způsob strunktruy (kompozice) nám umožnuje rychle vytvářet, nebo editovat velké množství jiných komponentů které pak tvoří scény nebo jiné objekty.
Když teda změníme v *missale_louncher* parametr že nevidí přez zdi tak všechny momenty kdy byl *missale_louncher* použit tak nebude vidět přez stěny.
### zacházení s komponenty
komopnenty by by neměli být přímíma childama hostatních komponentů, jediná výmky je u move_componentu
jestli chcete vytvořit kanón (který nemá kolize) bude vycházet s [Node2D](https://docs.godotengine.org/en/4.1/classes/class_node2d.html) ve které by se poté nacházel [sprite2D]() ,gun_component a rotate_to_component.
S tímhlě celím útvarem poté můžete pracovat jako s novím komponentem a několikrát ho dát do další [Node2D](https://docs.godotengine.org/en/4.1/classes/class_node2d.html) ke které předáte další rotate_to_component a vznikne vám kulometná věž s autonomníma dělama (pamatujte že [collision_shape](https://docs.godotengine.org/en/4.0/tutorials/physics/collision_shapes_2d.html) nemusí být vždycky kruh, a že se také rotuje podle parenta což může zajímavé řetězcovité effekty)
### move_component
Tohle teoreticky není komopnenent ale Root Noda, prosím nepřidávejte ji jako child nody k ostatním, věcem co mají funkce, mohlo by to ovlivnit funkčnost celého útvaru
Je komponent který je rootem každé objektu se kterým by mělo jít interagovat (pohybem atd.),komponent je viděděný z [RigidBody2D](https://docs.godotengine.org/en/4.0/classes/class_rigidbody2d.html) a řeší kolize, zrichlování a zvuky okolo pohybu
Aby tento kompont fungoval musíte přidat [collision_shape](https://docs.godotengine.org/en/4.0/tutorials/physics/collision_shapes_2d.html) který budou kolize objktu
### health_component
Využití tohohle komponentu nebylo na max, neboť životy (víc jak 1 život) jsme vyškrtly z plánu neboť neseděli do principu naší hry (jeď a nezastavuj)
Tenhle komponent by jinak má za úkol mazání not ze scény, ubírání životu, řešení nesmrtelnosti.
Poté na smrti spawne určený objekt se stejnou rotací a pozicí jak objekt co "zemřel", tohle se v projektu využívá na spawnování explozí
Měl mít ještě možnost spawnovat ragdolls kde by zkopírovala *visual_component* ale to byla následně zaohzeno kvuli zasazení
### visual_component
využítí mělo být na měnění barev, viditelnosti a dalších věci
nakonec bylo využite pouze na změnění barvy při ztrátě života
### gun_component
jestli se hráč (nebo jakákoliv nota nastavená) se octne v dosahu tak začne spawnovat určené objekty, tento komponent se má nejčastěj kombinovat s *rotate_to_component*
je dodatečné nastavení k toumu aby target byl zaregistrován i přez stěny
Aby tento kompont fungoval musíte přidat [collision_shape](https://docs.godotengine.org/en/4.0/tutorials/physics/collision_shapes_2d.html) kterým je vyhrazeno pole účinění
### rotate_to_component
komponent rotuje parenta na směrem k pozici nastavené noty, tato komponent má nastavení o kolik stupnů má mířít vedle pozice terče, umí započítat do toho i jeho rychlost (předměřování  je ujetá vzdálenost za sekundu)
je dodatečné nastavení k toumu aby target byl zaregistrován i přez stěny
Aby tento kompont fungoval musíte přidat [collision_shape](https://docs.godotengine.org/en/4.0/tutorials/physics/collision_shapes_2d.html) kterým je vyhrazeno pole účinění
### player_component
podobá se rotatte_to_component, ale má být dán poze na hřáče, tenhle komponent rotuje parenta podle inputů které jsou zadány
### push_feald
konsistěntě odtlačuje všechny move_component směrem k [marker_2d](https://docs.godotengine.org/en/stable/classes/class_marker2d.html) jestli neekzistuje tak ke středu
jestli nastaveno nebude push_feald vtahovat ale tlačit všechny move_component od středu k [Markru_2D](https://docs.godotengine.org/en/stable/classes/class_marker2d.html) kterým je vyhrazeno pole účinění
je dodatečné nastavení k toumu aby target byl zaregistrován i přez stěny
### spawner_component
spawnuje v uvedeném intervalu konzistentně objekty
### replay_component
ma zaůkol přehrávání i nahrávání replayů ty nahrává do formátu classy [RecordFormat](####RecordFormat)
## privky mapy
### [tile map](https://docs.godotengine.org/en/3.5/tutorials/2d/using_tilemaps.html)
v projektu máme jen jeden druch tile mapy (hexagony) 

### end (finish, end goal)
je konec mapy který zajištuě zplnění kondicí, pripravuje a náhle i  zpouští funkce pro ukládání replaye (``replay_handler.AutoSave()``) jestli to ovšem nebyl přehráný replay (můžete přehrát i replay co není váš)
#### flags
přidávájí možnost povinných bodů na projetí nebo vytvoření několika okruhovích map
##### použítí
zkládá se ze 2 noudů, flag a flag_control
flag_control by měl být ve scéně parentem **flag**ům, v **flag_control**u se nastavuje počet kola
kdyždý flag potřebuje svůj vlastní [collision_shape](https://docs.godotengine.org/en/4.0/tutorials/physics/collision_shapes_2d.html)
## Replays
Celí replay systém stojí na jedné (pověčinou statické) classe replay_handler, která zajištujě serializace, de-serializaci (.Net knihovna) .json s replayem, jejich ukládání, ukládání nejlepších skére i přípravu pro následné přehrávání jakýkoliv uloženého replaye
### Popsání duležitých funkcí a struktury
#### public static void SaveReplay([String](https://docs.godotengine.org/en/stable/classes/class_string.html))
jako paramet bere jméno (cestu + jméno) ukládaného replaye, všechny ovšem začínají na "user://replays/" a funkce automaticky přidá koncovku ".json" -> ``SaveReplay("Test")`` vytvoří se replay user://replays/Test.json
je to jediná funkce na ukládání replayů, ukládá replay který je aktuálně načtený v buffru (``RecordFormat[] replay_handler.bufferReplay``), tudíš kdykoliv potřebujete vytvořit novu funkci na ukládání napřed načtěne replay do buffru a pak zpuště tuto funkci
všechny funkce které mají za úkol ukládat (``AutoSave()``, ``SaveReplay()``) jdou též přez tuto funci
tato funkce využívá .Net knihovny [JsonClass](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject?view=net-8.0)
#### public static JsonNode GetJson([String](https://docs.godotengine.org/en/stable/classes/class_string.html))
funkce bere jako argument jméno (nebo cestu + jméno, stejně jako funkce [SaveReplay](####SaveReplay) začíná v "user://replays/" a  automaticky přidává příponu) uloženého replaye a z plain textu ho předělá do .Net [Json objektu](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject?view=net-8.0) který poté returne
jestli ovšem soubor neekzistuje returne prázdný [Json objekt](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject?view=net-8.0) ``return JsonNode.Parse("");`` 
konečný [Json objekt](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject?view=net-8.0) má 
```json
{
    "replay":   [{},{},{},{},{},{},{},{},{}], // byly by formatovane RecordFormat objekty
    "user":     "Godot",
    "user_id":  00000001,
    "map":      "Easy/enter_the_grid", //(map path)
    "date":     "5/21/2024 10:57:42AM", //(datum UTC)
    "tps":      100, //(tick per second)
    "ticks":    512, //(celková délka replaye)
}
```
#### public static [RecordFormat](####RecordFormat)[] GetReplay([String](https://docs.godotengine.org/en/stable/classes/class_string.html))
funkce bere jako argument jméno (nebo cestu + jméno, stejně jako funkce [SaveReplay](####SaveReplay) začíná v "user://replays/" a  automaticky přidává příponu)
využívá funkce [GetJson](####test) který potom oddělá nepotřebné informace jako je mapa, id uživatele, jméno uživatele, datum, čas, tick speed a pomocí konstruktoru [RecordFormat](####RecordFormat)u na [Json objekty](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject?view=net-8.0) vytvořý replay který returne
#### RecordFormat
Je jednoducá classa do která drží informace hráče v jednotlivích framech
```cs
	public bool active {get; set;}

	public float rotation {get; set;}
	public float rotationVel {get; set;}

	public float positionX {get; set;}
	public float positionY {get; set;}

	public float velocityX {get; set;}
	public float velocityY {get; set;}
```
má 2 construktory jeden "normální" (bool, 6x float) a jeden využívajcí .Net knihovet který bere jako parametr speciálně konstruktovaný [Json Objekt](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject?view=net-8.0)
pozice ani rychlost nejsou uloženy v [Vector2](https://docs.godotengine.org/en/3.1/classes/class_vector2.html) protože .Net nemá v základu Godot classy a i kdyby byly použité oficiální funkce godotu tak [Vecotr2 nemají podporovaný převod do jsnu](https://docs.godotengine.org/en/latest/tutorials/io/saving_games.html#serializing)
tento formát byl určet i k využití na multiplayer servrech neboť je to classa která bude zdílena stejně mezi serverem a klientem a obsahuje všechni informace které by byly potřeba při ktátkých výstřelů pingu

