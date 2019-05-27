# ML_Images

# Liczenie odległości

# Łączenie punktów w podobne pary

# Usuwanie szumu

# Spójne sąsiedztwo:
Para (Pi,Qi). Załóżmy, że w okolicy znajdują się inne punkty kluczowe. Przyjmujemy rozmiar sąsiedztwa (n). Patrzymy na n najbliższych punktów na tym samym obrazie. Patrzymy ile par punktów jest wzajemnie w tym sąsiedztwie. Liczymy procentową ilość tych punktów i porównujemy z ustalonym progiem.

# Uczenie - modelowanie przekształceń

# 1. D = zbiór par punktów kluczowych (Pi, Qi) o liczności m.
# Znajdujemy przekształcenie A -> A Pi = Qi, A - macierz
A - transformata afiniczna - obrót, przesunięcie
a b c
d e f
0 0 1
P
x
y
1
3 punkty - 6 równań

# 2. Perspektywa
H - transformata perspektywiczna
a b c
d e f
g h 1
4 punkty - 8 równań

# Wybór punktów - RANSAC
Stąd mamy przekształcenie A* i H* - za ich pomocą usuwamy więcej nadmiarowych punktów

# Teraz przekształcenie
A* Pi = Qi'
Badamy odległość między Qi oraz Qi'. Jeśli większa od progu delta, to coś jest nie tak. Wyrzucamy parę punktów.
