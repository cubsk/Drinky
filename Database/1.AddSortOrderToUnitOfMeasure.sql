alter table UnitOfMeasure add SortOrder int


update UnitOfMeasure set SortOrder = 1 where Name = 'Ounce'
update UnitOfMeasure set SortOrder = 2 where Name = 'Tablespoon'
update UnitOfMeasure set SortOrder = 3 where Name = 'Teaspoon'
update UnitOfMeasure set SortOrder = 4 where Name = 'Dash'
update UnitOfMeasure set SortOrder = 5 where Name = 'Unit'

alter table UnitOfMeasure alter column SortOrder int not null

