INSERT INTO TipoFiltro (Id, Expresion) VALUES
(1, 'Mayor que'),
(2, 'Menor que'),
(3, 'Mayor igual que'),
(4, 'Menor igual que'),
(5, 'igual');




INSERT INTO SuperficieFiltro (id,Descripcion, IdTipoFiltro,Valor, Borrado, Editable) VALUES
(1,N'Con datos',1,N'0',0,0),
(2,N'Sin datos',5,N'0',0,0),
(3,N'Menos de 1ha',4,N'1',0,1),
(4,N'Más de 1ha',1,N'1',0,1),
(5,N'Más de 500ha',1,N'500',0,1);




