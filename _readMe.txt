��� - ������������� ����� ������ ��������� ��� Windows � �� ADS #xml.
������ ������: 15 ������� 2023 ����.
----------------------------------------------------------------------------
- Scanword.dll - ������������ �� ������� ScanWordMobile

- ������ ��-���� ��������� �� ���� �� SQLite � desktop ����������
  - ���������� ������� � SQLite �� ������� valpost ��� �������� ����� -> ��������� �� SQLite
  - ���������� ���������� ����� � �� -> ��������� �� SQLite

- �� ������ ����������� � ������� -> �������, �� ����� ��������� ��� ���������� ����� ����!

- �������� �������� DataGridView:
https://www.cyberforum.ru/ado-net/thread1890119.html
���������� � ��� ����.... ��������� ��� ����� ���� ������ ������:
dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
� ���������� �������� �� ����������� �������.
---
� ���� ������ ���� ���� ������:
this._wordsDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
this._rusDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
this._dictDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;


TODO:
-----
- ��� ChangesFromServer (��������� � �������) ���������� ������� letters (����� �� ������) - 
  ����� ���� ��� ����� ������ �������� - �������� � ��������� 618 (auto-put).
  ������. ������� ��� ��������� � 0 � ������� nchanges.
  �������: ����������� 'D bratanjac' �� ��������, ��� ��� �� ����� ���� ��� ��������� '������|��'. �������� ������� � ��.
  �� ������ ������ ������������� 643 � �� 45649 ����. � � ��������� ���������� (1.0.0) 45647 ����. ���� �����������, ������ ���.

- ��� ChangesFromServer ��������� ��������� ��� ���������� ���������

- ������� ����������� ��������� ��������� � ������� �� ����

- �������� �� � ������������ � �� ���������� ���������� (�������� � ������� ������)

- ������� ������� 

- ���������: ����� �� �������� ����� ������? ������� ������� ��� ���������� ��������� ����� ��� ���� (3) ��������

- ������� ������� � ��� �������� �������