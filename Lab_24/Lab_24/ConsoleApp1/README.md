# Lab24 — Strategy + Observer (C#)

##  Тема

**Strategy + Observer: динамічна підстановка алгоритмів + події + тести**

## Мета роботи

Застосувати патерни **Strategy** та **Observer** для створення гнучкої системи обробки числових даних, яка дозволяє:

* Динамічно змінювати алгоритми обробки.
* Автоматично сповіщати залежні компоненти про результати.
* Демонструвати взаємодію компонентів у реальному часі.

---

##  Структура проєкту

```
lab24
 ├─ Strategies
 │   ├─ INumericOperationStrategy.cs
 │   ├─ SquareOperationStrategy.cs
 │   ├─ CubeOperationStrategy.cs
 │   └─ SquareRootOperationStrategy.cs
 │
 ├─ Core
 │   ├─ NumericProcessor.cs
 │   └─ ResultPublisher.cs
 │
 ├─ Observers
 │   ├─ ConsoleLoggerObserver.cs
 │   ├─ HistoryLoggerObserver.cs
 │   └─ ThresholdNotifierObserver.cs
 │
 └─ Program.cs
```

---

#  Реалізація патернів

##  Strategy

### Інтерфейс

`INumericOperationStrategy`

```csharp
double Execute(double value);
```

Дозволяє визначити спільний контракт для всіх алгоритмів.

### Реалізації

| Стратегія                   | Опис              |
| --------------------------- | ----------------- |
| SquareOperationStrategy     | Квадрат числа     |
| CubeOperationStrategy       | Куб числа         |
| SquareRootOperationStrategy | Квадратний корінь |

### Context

`NumericProcessor`

Функції:

* Приймає стратегію через конструктор.
* Дозволяє змінювати її через `SetStrategy()`.
* Виконує обробку через `Process()`.

---

##  Observer

Реалізований через **події C#**.

### Publisher (Subject)

`ResultPublisher`

Подія:

```csharp
public event Action<double, string> ResultCalculated;
```

Метод:

```csharp
PublishResult(double result, string operationName);
```

---

##  Спостерігачі

### ConsoleLoggerObserver

Виводить результат у консоль.

### HistoryLoggerObserver

Зберігає історію обчислень у `List<string>`.

### ThresholdNotifierObserver

Сповіщає, якщо результат перевищує заданий поріг.

---

##  Приклад виводу

```
=== Square ===
[Console] Operation: Square | Result: 4
[Console] Operation: Square | Result: 9

[Threshold] Result 64 exceeded threshold 50
```


#  Як запустити проєкт

1. Клонувати репозиторій:

```
git clone https://github.com/USERNAME/lab24.git
```

2. Відкрити у Visual Studio / Rider / VS Code.
3. Запустити проєкт:

```
dotnet run
```
