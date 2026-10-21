# Notatki

### Cośtam

`ref` - struktura może być alokowana wyłącznie na stosie

`readonly` - gwarantuje, że oznaczony nim element (zmienna, właściwość lub cała struktura) może otrzymać wartość tylko jeden raz podczas tworzenia w konstruktorze, po czym zostaje trwale zablokowany przed jakimikolwiek zmianami

---

### Interface

Interfejs to nic innego jak **pusty kontrakt**.   
Kiedy przed chwilą rozmawiałyśmy o klasach abstrakcyjnych (Pojazd), określałyśmy to, **kim ktoś jest** (Samochód *jest* Pojazdem). Z kolei interfejs określa wyłącznie to, **co dany obiekt potrafi robić**, całkowicie ignorując to, czym ten obiekt fizycznie jest.   
Najlepszą analogią w prawdziwym świecie jest standard USB. Klawiatura, pendrive, myszka i drukarka to zupełnie inne przedmioty, służące do innych rzeczy (miałyby inne klasy bazowe). Ale wszystkie one "podpisały kontrakt" o nazwie `IUsbCompatible`. Komputer nie musi wiedzieć, czym jest dane urządzenie – widzi certyfikat USB, więc wie, że może z nim rozmawiać.

**1. Kontrakt (Interfejs `IEnumerator`)**

```csharp
public interface IEnumerator //I - od interface
{
    bool MoveNext();
    object Current { get; }
    void Reset();
}
```

Ten interfejs to dosłownie ogłoszenie o pracę, które mówi: *"Jeśli jakakolwiek klasa chce mieć certyfikat IEnumerator, musi posiadać trzy rzeczy: funkcję przechodzenia dalej, właściwość pokazującą obecny element i funkcję resetu"*.   
Nie ma tu żadnych klamerek `{}` z logiką. Interfejs nie mówi **jak** to zrobić. Mówi tylko, **co** musi być zrobione.

**2. Podpisanie kontraktu (Klasa `Counter`)**

```csharp
public class Counter : IEnumerator
```

Klasa `Counter` (Licznik) staje do walki i dopisuje po dwukropku `: IEnumerator`. Tym samym oficjalnie podpisuje umowę. Od tego momentu kompilator C# staje się bezlitosnym inspektorem. Zmusza klasę `Counter` do fizycznego napisania (zaimplementowania) wszystkich trzech wymaganych elementów.   
Gdybyś w klasie `Counter` napisała logikę dla `MoveNext` oraz `Current`, ale zapomniałabyś o `Reset`, kompilator natychmiast podkreśliłby klasę na czerwono, krzycząc: *"Obiecałaś spełnić wymagania interfejsu IEnumerator, a brakuje ci funkcji Reset!"*.
*(Dlatego w Twoim kodzie twórca napisał `Reset()`, nawet jeśli w środku wyrzuca tylko błąd `NotSupportedException` – musiał dostarczyć funkcję, żeby spełnić kontrakt).*

**Po co nam interfejsy, skoro mamy klasy abstrakcyjne?**

To najważniejsze pytanie. W języku C# obowiązuje żelazna zasada: **klasa może mieć tylko jednego biologicznego rodzica** (może dziedziczyć tylko po jednej klasie bazowej). Twój `Car` nie mógłby dziedziczyć jednocześnie po `Vehicle` i po `Weapon`.   
Ale klasa może podpisać **dowolną liczbę interfejsów**.
Dzięki temu możesz zbudować obiekt tak:
`public class Car : Vehicle, IDrivable, IWashable, ISellable`.   
Dzięki temu silnik fizyczny potraktuje go jako coś, co umie jeździć (`IDrivable`), myjnia potraktuje go jako obiekt, który da się umyć (`IWashable`), a sklep potraktuje go jako towar, który ma cenę (`ISellable`). Interfejsy pozwalają łączyć zupełnie różne obiekty w grupy na podstawie tego, co potrafią zrobić.

**Dlaczego trzeba napisać List<IHittable> Objects?**

Twoja klasa Scene to kontener na wszystko, co ma zostać narysowane na ekranie. Gdybyś napisała List<Sphere>, Twój worek akceptowałby wyłącznie kule. Gdybyś jutro chciała dodać do swojego raytracera płaską podłogę (Plane) albo trójkąt (Triangle), kompilator by na to nie pozwolił.

Używając interfejsu jako typu w liście: List<IHittable>, Twój worek (lista) zyskuje swojego własnego Bramkarza. Ten bramkarz krzyczy: "Nie obchodzi mnie, czy jesteś kulą, płaszczyzną, trójkątem czy importowanym modelem 3D Wiedźmina. Jeśli tylko masz na ręce pieczątkę IHittable (czyli potrafisz przyjąć na siebie uderzenie promienia), wchodzisz do worka!"

To kwintesencja programowania obiektowego (tzw. polimorfizm). Twój raytracer będzie potem miał jedną, prostą pętlę przelatującą przez ten worek.

Oto zwięzła notatka, która zbiera Twoje materiały z wykładów i nasze wspólne wnioski. Idealna do szybkiej powtórki przed kolokwium!

---

### Przekazywanie parametrów przez referencję w C# (`ref`, `out`, `in`)

**Po co to robimy?**

Zwykłe struktury (`struct`, np. `Vector3`, `HitInfo`) to typy wartościowe. Domyślnie przy przekazywaniu do metody C# robi ich kopię ("ksero"). Przy dużych strukturach pożera to pamięć i czas. Użycie referencji sprawia, że przekazujemy tylko **adres w pamięci** (jak wskaźniki w C/C++), unikając niepotrzebnego kopiowania.

**1. `ref` (Dwukierunkowa referencja – "Odczytaj i popraw")**

* **Zasada:** Zmienna **musi** zostać zainicjowana przed przekazaniem do metody.
* **Dlaczego?** Ponieważ metoda może chcieć od razu z niej coś odczytać (nie może tam być "śmieci" z pamięci).
* **Działanie:** Metoda może odczytać starą wartość, zmienić ją i zapisać nową.
* **Przykład:** Funkcja zamieniająca dwie liczby miejscami `Swap(ref int a, ref int b)`.

**2. `out` (Argument wyjściowy – "Puste pudełko do wypełnienia")**

* **Zasada:** Zmienna przed wejściem nie musi mieć żadnej wartości (można wręcz stworzyć nową w locie). Jednak metoda **ma obowiązek** coś do niej przypisać przed zakończeniem (słowem `return`). Kompilator tego pilnuje.
* **Po co?** Służy do zwracania "tylnymi drzwiami" dodatkowych informacji, gdy główny `return` jest już zajęty.
* **Przykład:** Metoda `Hit` zwraca przez główny `return` samo `true`/`false` (czy trafiono), a przez `out HitInfo hitInfo` wyrzuca nową strukturę z dokładnymi danymi zderzenia.
* *Trik C#:* Zmienną można utworzyć bezpośrednio w nawiasach: `Hit(..., out HitInfo raport);`.

**3. `in` (Tylko do odczytu – "Patrz, ale nie dotykaj")**

* **Zasada:** Gwarantuje absolutny brak modyfikacji argumentu wewnątrz metody (odpowiednik `const Type&` z C++).
* **Po co?** Używane wyłącznie dla optymalizacji. Przekazujemy ogromną strukturę (np. wielką macierz matematyczną), żeby uniknąć jej powolnego kopiowania, ale metoda ma zakaz jej edytowania. Próba zmiany wywali błąd kompilatora.
