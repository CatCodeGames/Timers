# 1. Description
An efficient and accurate timer for Unity with PlayerLoop integration. The timer accounts for time differences between cycles, avoiding accumulation errors. 
Key features:
- setting the number of repetitions;
- selecting the game update cycle (Update, FixedUpdate, or LateUpdate);
- the ability to consider or ignore time scaling;
- support for multiple invocations on each timer trigger;
- safe multiple starting and stopping of the timer within a single update cycle.

# 2. Usage
### Creation
To create a timer object, use the constructor with the following parameters:
``` csharp
IntervalTimer timer = new IntervalTimer(interval, callback, updateMode, unscaledTime, loops, multiInvokeOnUpdate)
```
- interval: Time in seconds between timer triggers.
- callback: Method called when the timer triggers.
- updateMode: Unity update cycle in which the timer will be processed (Update, FixedUpdate, or LateUpdate). Default is Update.
- unscaledTime: Whether to consider time scaling. Default is false.
- loops: Number of timer repetitions. Set to -1 for an infinite timer. Default is -1.
- multiInvokeOnUpdate: Whether to allow multiple callback invocations per update cycle. Default is false.

### Control
- Start(): Starts the timer.
- Stop(): Stops the timer.

The Start and Stop methods can be safely called multiple times for the timer within a single update cycle. The timer correctly handles such multiple calls in one frame.

# 3. Example
``` csharp
    private void Awake()
    {
        _timer = new IntervalTimer(1f, OnElapsed);
    }

    private void OnEnable()
    {
        _timer.Start();
    }

    private void OnDisable()
    {
        _timer.Stop();
    }

    private void OnElapsed()
    {
        Debug.Log($"{Time.time}");
    }
```

Example Output
``` csharp
    1,003953
    2,002245
    3,00188
    4,004385
    5,004099
    ...
```



# 1. Описание
Эффективный и точный таймер для Unity с интеграцией в PlayerLoop. Таймер учитывают разницу времени между циклами, избегая накопления ошибок. 
Основные возможности:
- задание количества повторений;
- выбор игрового цикла обновления (Update, FixedUpdate или LateUpdate);
- возможность учитывать или игнорировать масштаб времени;
- поддержка множественных вызовов при каждом срабатывании таймера;
- безопасный многократный запуск и остановка таймера в пределах одного цикла обновления;

# 2. Использование
### Создание
Для создание объекта таймера используется конструктор со следующими параметрами.
``` csharp
IntervalTimer timer = new IntervalTimer(interval, callback, updateMode, unscaledTime, loops, multiInvokeOnUpdate)
```
- interval: Время в секундах между срабатываниями таймера.
- callback: Метод, вызываемый при срабатывании таймера.
- updateMode: В каком цикле обновления Unity будет обрабатываться таймер (Update, FixedUpdate или LateUpdate). По умолчанию - Update
- unscaledTime: Учитывать ли масштаб времени. По умолчанию - false
- loops: Количество повторений таймера. Для бесконечного таймера указать параметр -1. По умолчанию -1.
- multiInvokeOnUpdate: Разрешить ли множественные вызовы callback за один цикл обновления. По умолчанию false.

### Управление
- Start(): Запуск таймера.
- Stop(): Остановка таймера.

Методы Start и Stop могут быть безопасно вызваны несколько раз для таймера в пределах одного цикла обновления. Таймер корректно обрабатывает такие множественные вызовы в одном кадре.


# 3. Пример
``` csharp
    private void Awake()
    {
        _timer = new IntervalTimer(1f, OnElapsed);
    }

    private void OnEnable()
    {
        _timer.Start();
    }

    private void OnDisable()
    {
        _timer.Stop();
    }

    private void OnElapsed()
    {
        Debug.Log($"{Time.time}");
    }
```

Пример вывода
``` csharp
    1,003953
    2,002245
    3,00188
    4,004385
    5,004099
    ...
```
