# Notatki

`ref` - struktura może być alokowana wyłącznie na stosie

`readonly` - gwarantuje, że oznaczony nim element (zmienna, właściwość lub cała struktura) może otrzymać wartość tylko jeden raz podczas tworzenia w konstruktorze, po czym zostaje trwale zablokowany przed jakimikolwiek zmianami

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
`public class Car : Vehicle, IDrivable, IWashable, ISellable`

Dzięki temu silnik fizyczny potraktuje go jako coś, co umie jeździć (`IDrivable`), myjnia potraktuje go jako obiekt, który da się umyć (`IWashable`), a sklep potraktuje go jako towar, który ma cenę (`ISellable`). Interfejsy pozwalają łączyć zupełnie różne obiekty w grupy na podstawie tego, co potrafią zrobić.
