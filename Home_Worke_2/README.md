# Порушення принципу підстановки Лісков (LSP)

## Загальна характеристика принципу LSP

Принцип підстановки Лісков (Liskov Substitution Principle) визначає, що об’єкти похідних класів повинні без проблем замінювати об’єкти базових класів. Якщо програма коректно працює з базовим класом, вона повинна так само коректно працювати з будь-яким його нащадком без зміни логіки.

Порушення LSP призводить до неочікуваної поведінки програми, появи винятків під час виконання та ускладнення підтримки коду.



## Приклад 1. Клас Bird та клас Penguin

### Опис порушення

```csharp
class Bird
{
    public virtual void Fly()
    {
        Console.WriteLine("Bird is flying");
    }
}

class Penguin : Bird
{
    public override void Fly()
    {
        throw new NotSupportedException("Penguins can't fly");
    }
}
```

Базовий клас `Bird` визначає поведінку польоту, припускаючи, що кожен птах може літати. Клас `Penguin` наслідує `Bird`, але не підтримує метод `Fly` та кидає виняток.

### Чому це порушує LSP

Код, який працює з типом `Bird`, очікує, що метод `Fly` буде виконуватись коректно. Підміна об’єкта `Bird` на `Penguin` призводить до помилки під час виконання, що порушує принцип підстановки.

### Проблеми, які виникають

- неочікувані винятки;
- необхідність перевірки конкретного типу об’єкта;
- порушення поліморфізму.

### Варіант перепроєктування

```csharp
abstract class Bird
{
    public abstract void Move();
}

interface IFlyingBird
{
    void Fly();
}

class Sparrow : Bird, IFlyingBird
{
    public override void Move()
    {
        Fly();
    }

    public void Fly()
    {
        Console.WriteLine("Sparrow is flying");
    }
}

class Penguin : Bird
{
    public override void Move()
    {
        Console.WriteLine("Penguin is swimming");
    }
}
```

У даному варіанті поведінка польоту винесена в окремий інтерфейс, який реалізують лише ті класи, що дійсно підтримують цю можливість.



## Приклад 2. FileLogger та ReadOnlyLogger

### Опис порушення

```csharp
class FileLogger
{
    public virtual void Write(string message)
    {
        Console.WriteLine("Writing to file: " + message);
    }
}

class ReadOnlyLogger : FileLogger
{
    public override void Write(string message)
    {
        throw new InvalidOperationException("Logger is read-only");
    }
}
```

Клас `ReadOnlyLogger` наслідує `FileLogger`, але забороняє виконання методу запису, порушуючи очікувану поведінку базового класу.

### Чому це порушує LSP

Будь-який об’єкт типу `FileLogger` повинен підтримувати запис повідомлень. Підстановка `ReadOnlyLogger` замість `FileLogger` призводить до помилки, що порушує контракт базового класу.

### Проблеми, які виникають

- аварійне завершення програми;
- складність розширення системи логування;
- залежність клієнтського коду від конкретних реалізацій.

### Варіант перепроєктування

```csharp
interface ILogger
{
    void Log(string message);
}

class FileLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine("Writing to file: " + message);
    }
}

class ReadOnlyLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine("Read-only log: " + message);
    }
}
```

У цьому випадку всі реалізації інтерфейсу `ILogger` гарантують коректну роботу методу `Log`.



## Приклад 3. Account та FixedDepositAccount

### Опис порушення

```csharp
class Account
{
    public virtual void Withdraw(decimal amount)
    {
        Console.WriteLine($"Withdrawn {amount}");
    }
}

class FixedDepositAccount : Account
{
    public override void Withdraw(decimal amount)
    {
        throw new InvalidOperationException("Withdrawal not allowed");
    }
}
```

Клас `FixedDepositAccount` забороняє операцію зняття коштів, хоча базовий клас `Account` визначає цю можливість як обов’язкову.

### Чому це порушує LSP

Клієнтський код очікує, що будь-який об’єкт типу `Account` дозволяє зняття коштів. Підстановка `FixedDepositAccount` призводить до порушення цієї гарантії.

### Проблеми, які виникають

- порушення бізнес-логіки;
- неочікувані винятки;
- ускладнення підтримки коду.

### Варіант перепроєктування

```csharp
abstract class Account
{
    public decimal Balance { get; protected set; }
}

interface IWithdrawable
{
    void Withdraw(decimal amount);
}

class SavingsAccount : Account, IWithdrawable
{
    public void Withdraw(decimal amount)
    {
        Balance -= amount;
    }
}

class FixedDepositAccount : Account
{
}
```

Зняття коштів винесено в окремий інтерфейс, який реалізують лише ті типи рахунків, для яких ця операція дозволена.



## Висновок

Порушення принципу підстановки Лісков виникає тоді, коли похідний клас змінює або обмежує поведінку базового класу. Для дотримання LSP слід використовувати інтерфейси, абстракції та чітко визначені контракти між класами. Це дозволяє створювати гнучкий, передбачуваний та підтримуваний код.

