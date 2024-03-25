CREATE TABLE DatePersoane (
    Universitate NVARCHAR(100),
    An INT,
    Specializare NVARCHAR(100),
    Grupa NVARCHAR(10),
    Media DECIMAL(5,2),
    DataNastere DATE,
    CodPersoana INT,
    FOREIGN KEY (CodPersoana) REFERENCES Persoane(Cod)
);
