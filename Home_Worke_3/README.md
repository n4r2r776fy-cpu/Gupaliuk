# Практична робота 3: Принципи ISP та DIP

## 1. Принцип ISP (Interface Segregation Principle)

**Суть:**  
Клас не повинен залежати від методів, які він не використовує. Інтерфейси повинні бути **«вузькими»** та спеціалізованими, а не загальними.

### Приклад порушення ISP

```csharp
interface IPrinter {
    void Print(Document doc);
    void Scan(Document doc);
    void Fax(Document doc);
}

class OldPrinter : IPrinter {
    public void Print(Document doc) { /* реалізація */ }
    public void Scan(Document doc) { throw new NotImplementedException(); }
    public void Fax(Document doc) { throw new NotImplementedException(); }
}
```

**Проблема:**  
Клас `OldPrinter` змушений реалізовувати методи `Scan` та `Fax`, хоча вони йому не потрібні. Це порушує принцип ISP.

### Рішення

```csharp
interface IPrinter {
    void Print(Document doc);
}

interface IScanner {
    void Scan(Document doc);
}

interface IFax {
    void Fax(Document doc);
}

class OldPrinter : IPrinter {
    public void Print(Document doc) { /* реалізація */ }
}
```

**Перевага:**  
- Класи реалізують тільки ті методи, які їм потрібні  
- Інтерфейси стають **чіткими та «вузькими»**  
- Легше тестувати та підтримувати код

---

## 2. Принцип DIP (Dependency Inversion Principle)

**Суть:**  
Класи високого рівня не повинні залежати від класів низького рівня. Всі повинні залежати від **абстракцій (інтерфейсів або абстрактних класів)**.

### Приклад застосування DIP через Dependency Injection

```csharp
interface IMessageService {
    void SendMessage(string message);
}

class EmailService : IMessageService {
    public void SendMessage(string message) { Console.WriteLine("Email: " + message); }
}

class Notification {
    private readonly IMessageService _service;

    // Dependency Injection через конструктор
    public Notification(IMessageService service) {
        _service = service;
    }

    public void Notify(string message) {
        _service.SendMessage(message);
    }
}
```

**Переваги DIP + DI:**  
- Можна легко змінити реалізацію сервісу (`EmailService`, `SMSService`) без зміни класу `Notification`  
- Легко підставляти **моки** або заглушки для тестування  
- Код стає більш гнучким та розширюваним

---

## 3. Взаємозв’язок ISP та DI

- **Вузькі інтерфейси (ISP)** роблять абстракції чистими і спеціалізованими  
- Це дозволяє **легко підставляти конкретні реалізації через DI**, не змушуючи клас залежати від зайвих методів  
- Як результат:  
  - Легше писати **unit-тести**  
  - Зменшується кількість залежностей  
  - Підвищується підтримуваність коду

---

## Висновок

Застосування **ISP** та **DIP** разом дозволяє створювати чистий, гнучкий та тестований код. «Вузькі» інтерфейси + Dependency Injection — це основа якісної об’єктно-орієнтованої архітектури.

