-- Nettoyage avant réinsertion
TRUNCATE TABLE Product;
TRUNCATE TABLE Category;

-- Catégories
INSERT INTO Category ([Name]) VALUES
('Informatique'),
('Électronique'),
('Maison'),
('Livres'),
('Vêtements');

-- Produits (Price en centimes, ex: 89999 = 899,99 €)
INSERT INTO Product ([Name], [Description], [Price], [CategoryId]) VALUES
('Ordinateur portable', 'Laptop 15 pouces avec SSD et 16Go RAM', 89999, 1),
('Clavier mécanique', 'Clavier gamer RGB switches bleus', 12999, 1),
('Souris sans fil', 'Souris ergonomique rechargeable', 4999, 1),

('Téléviseur 4K', 'TV UHD 55 pouces, HDR10+', 69999, 2),
('Casque Bluetooth', 'Casque audio sans fil avec réduction de bruit', 19999, 2),
('Smartphone Android', 'Smartphone 128Go, écran OLED', 59999, 2),

('Aspirateur', 'Aspirateur sans sac, 2000W', 14999, 3),
('Lampe LED', 'Lampe de bureau à intensité réglable', 3999, 3),

('Roman policier', 'Thriller captivant', 1499, 4),
('Livre technique C#', 'Apprenez C# et .NET Core', 3999, 4),

('T-shirt', 'T-shirt 100% coton', 1999, 5),
('Jean slim', 'Jean bleu slim fit', 4999, 5),
('Pull laine', 'Pull chaud en laine mérinos', 7999, 5);
