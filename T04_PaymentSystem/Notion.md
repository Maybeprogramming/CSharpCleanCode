### Попытка 1 - отклонено
Мой ответ: [GIT](https://github.com/Maybeprogramming/CSharpCleanCode/blob/a39bde3f3b28699f9dd7ee452788ea0a9dda09e9/T04_PaymentSystem/Program.cs)

Оценено в	вторник, 28 октября 2025, 11:58 - Денис Калужин

Отзыв в виде комментария	

Здравствуйте. Что-то вы перемудрили.

1. BasePaymentSystem - не нужен базовый класс. В реальности у систем не будет достаточно общего. Достаточно интерфейса. 
2. hash={{{hash}}}- "нужно больше скобок")) Зачем аж 3 пары?)
3. Секретный ключ в конструтор приходит. 
4. IPrivacyPolicy - очень путает название. IHasher какой-нибудь куда нагляднее.
5. CriptoSystem - чтобы что? Обертка ради обертки. Лишний класс. 
6. PaymentSystem - аналогично. Убираем.
7. Order - Не забываем про предыдущие уроки. Везде должны быть конструкторы и валидация входных параметров с необходимыми исключениями.

---
### Попытка 2 - принято
Мой ответ: [GIT](https://github.com/Maybeprogramming/CSharpCleanCode/blob/719b283cde8735b57f25692c37cc7542568aa5b1/T04_PaymentSystem/Program.cs)

Оценено в	вторник, 28 октября 2025, 13:37 - Денис Калужин

Отзыв в виде комментария	

Проверку на null можно сократить 
~~~csharp 
_warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));
~~~

---