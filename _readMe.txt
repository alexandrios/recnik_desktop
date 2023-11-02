Это - проектируемая новая версия программы для Windows с БД ADS #xml.
Начало работы: 15 октября 2023 года.
----------------------------------------------------------------------------
- Scanword.dll - используется из проекта ScanWordMobile

- Почему всё-таки отказался от идеи БД SQLite в desktop приложении
  - Необходимо сделать в SQLite БД таблицу valpost для проверки ключа -> отказался от SQLite
  - Необходимо шифрование полей в БД -> отказался от SQLite

- БД должна обновляться с сервера -> сделано, но нужно проверить при добавлении новых слов!

- Медленно грузится DataGridView:
https://www.cyberforum.ru/ado-net/thread1890119.html
Разобрался в чем дело.... Оказалось что нужно было просто убрать:
dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
И авторазмер поставил на необходимые столбцы.
---
В моем случае надо было убрать:
this._wordsDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
this._rusDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
this._dictDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;


TODO:
-----
- При ChangesFromServer (изменения с сервера) исправлять таблицу letters (поиск по буквам) - 
  после того как будет сделан алгоритм - прогнать с изменения 618 (auto-put).
  Сделал. Прогнал все изменения с 0 в таблице nchanges.
  Заметил: исправление 'D bratanjac' не проходит, так как на самом деле это ошибочное 'братан|ац'. Исправил вручную в БД.
  На момент номера синхронизации 643 в БД 45649 слов. А в мобильном приложении (1.0.0) 45647 слов. Надо разобраться, почему так.

- При ChangesFromServer проверить поведение при отсутствии интернета

- Сделать возможность проверять изменения с сервера из меню

- Привести БД в соответствие к БД мобильного приложения (ударения в русских словах)

- Сделать озвучку 

- Проверить: можно ли обойтись одним гридом? Сделать объекты для сохранения состояния грида для всех (3) словарей

- Сделать историю и для русского словаря