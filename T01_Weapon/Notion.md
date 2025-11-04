### Попытка 1 - отклонено

Оценено в	четверг, 11 апреля 2024, 19:38 - Алексей Кононов

Мой ответ: [Gist](https://gist.github.com/Maybeprogramming/e59d276b28e0a22ec270097d0cb901f0)

Отзыв в виде комментария 
Алексей, привет

```csharp
1.  _bullets -= 1; - 1 - магическое число

2.  class Weapon
    {
        private int _damage;
        private int _bullets; - тут нужен конструктор с агрегацией и проверкой значений

3.  if (damage < 0)
    {
        return false;
    } - в исключительных ситуациях не нужно использовать return, бросай исключение

4.  public void Initialize(Weapon weapon) - это должен быть конструктор с проверкой на null
```

Успехов

---
### Попытка 2 - отклонено

Оценено в	пятница, 17 октября 2025, 14:53 - Сергей Куделин

Мой ответ: [GIT](https://github.com/Maybeprogramming/CSharpCleanCode/blob/0d0ec7e05989614c9279fcc3a2488622d008233c/T01_Weapon/Task.cs)

Отзыв в виде комментария Йо! Исключения лучше бросать в блоке if, а не else. 

~~~csharp
 public void TryFire(Player player) - если пуль нет, следует бросать исключение
~~~

---
### Попытка 3 - принята

Мой ответ: [GIT](https://github.com/Maybeprogramming/CSharpCleanCode/blob/00015096b650f9271d61bc47262bec8ed6cdac31/T01_Weapon/Task.cs)

Оценено в	пятница, 17 октября 2025, 16:33 - Сергей Куделин

