# Lab25 — Інтеграція патернів проєктування

##  Тема

**Інтеграція патернів: Factory Method + Singleton + Strategy + Observer**

---

##  Мета роботи

Розробити програмну систему, яка демонструє спільну роботу кількох патернів проєктування:

* Factory Method
* Singleton
* Strategy
* Observer

Система повинна забезпечувати гнучку обробку даних, динамічну зміну алгоритмів та логування результатів із автоматичним сповіщенням компонентів.

---

#  Архітектура системи

У проєкті реалізовано інтегровану взаємодію:

* **LoggerManager (Singleton)** — централізоване керування логером.
* **LoggerFactory (Factory Method)** — створення типів логерів.
* **DataContext (Strategy)** — обробка даних за різними алгоритмами.
* **DataPublisher (Observer)** — сповіщення про завершення обробки.

---

#  Реалізація патернів

##  Factory Method

### Інтерфейс

`ILogger`

Метод:

```csharp
void Log(string message);
```

### Реалізації

| Клас          | Опис                     |
| ------------- | ------------------------ |
| ConsoleLogger | Логування в консоль      |
| FileLogger    | Логування у файл log.txt |

### Фабрики

| Фабрика              | Створює       |
| -------------------- | ------------- |
| ConsoleLoggerFactory | ConsoleLogger |
| FileLoggerFactory    | FileLogger    |

---

## Singleton

### LoggerManager

Особливості:

* Один екземпляр на всю програму.
* Ініціалізація фабрикою.
* Можливість зміни фабрики під час виконання.

Методи:

```csharp
Initialize(LoggerFactory factory);
ChangeFactory(LoggerFactory factory);
Log(string message);
```

---

## Strategy

### Інтерфейс

`IDataProcessorStrategy`

```csharp
string Process(string data);
string Name { get; }
```

### Реалізації

| Стратегія            | Опис                            |
| -------------------- | ------------------------------- |
| EncryptDataStrategy  | Проста шифрація (+1 до символу) |
| CompressDataStrategy | Видалення пробілів              |

### Context

`DataContext`

Функції:

* Приймає стратегію через конструктор.
* Дозволяє змінювати через `SetStrategy()`.
* Виконує обробку через `Execute()`.

---

## Observer

### Publisher

`DataPublisher`

Подія:

```csharp
public event Action<string, string> DataProcessed;
```

Викликається після обробки даних.

---

### Observer

`ProcessingLoggerObserver`

Функція:

* Підписується на подію.
* Використовує Singleton LoggerManager.
* Логує результат обробки.

---

#  Демонстраційні сценарії

##  Сценарій 1 — Повна інтеграція

1. LoggerManager → ConsoleLoggerFactory.
2. Strategy → EncryptDataStrategy.
3. Дані обробляються.
4. Publisher викликає подію.
5. Observer логує результат у консоль.

---

##  Сценарій 2 — Динамічна зміна логера

1. Початково → ConsoleLogger.
2. Після першої обробки → FileLogger.
3. Наступні логи записуються у файл `log.txt`.

---

##  Сценарій 3 — Динамічна зміна стратегії

1. Початково → Encryption.
2. Після зміни → Compression.
3. Дані обробляються за новим алгоритмом.

---

#  Приклад виводу

```
===== SCENARIO 1 =====
[Console Logger] Processed with Encryption: Ifmmp!Xpsme

===== SCENARIO 2 =====
(Check log.txt)

===== SCENARIO 3 =====
[File Logger] Processed with Compression: ObserverStrategyChanged
```

---

#  Запуск проєкту

1. Клонувати репозиторій:

```
git clone https://github.com/USERNAME/lab25.git
```

2. Відкрити у Visual Studio / VS Code.
3. Запустити:

```
dotnet run
```

---
#  Висновок

У лабораторній роботі:

* Реалізовано інтеграцію 4 патернів.
* Забезпечено гнучку зміну логування.
* Реалізовано зміну алгоритмів обробки.
* Налаштовано автоматичне сповіщення Observer.

Система демонструє ефективну взаємодію патернів проєктування у єдиній архітектурі.
