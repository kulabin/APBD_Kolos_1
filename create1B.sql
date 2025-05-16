-- tables
-- Table: Client
CREATE TABLE Client (
    client_id int  NOT NULL,
    first_name nvarchar(100)  NOT NULL,
    last_name nvarchar(100)  NOT NULL,
    date_of_birth datetime  NOT NULL,
    CONSTRAINT Client_pk PRIMARY KEY  (client_id)
);

-- Table: Mechanic
CREATE TABLE Mechanic (
    mechanic_id int  NOT NULL,
    first_name nvarchar(100)  NOT NULL,
    last_name nvarchar(100)  NOT NULL,
    licence_number nvarchar(14)  NOT NULL,
    CONSTRAINT Mechanic_pk PRIMARY KEY  (mechanic_id)
);

-- Table: Service
CREATE TABLE Service (
    service_id int  NOT NULL,
    name nvarchar(100)  NOT NULL,
    base_fee decimal(10,2)  NOT NULL,
    CONSTRAINT Service_pk PRIMARY KEY  (service_id)
);

-- Table: Visit
CREATE TABLE Visit (
    visit_id int  NOT NULL,
    client_id int  NOT NULL,
    mechanic_id int  NOT NULL,
    date datetime  NOT NULL,
    CONSTRAINT Visit_pk PRIMARY KEY  (visit_id)
);

-- Table: Visit_Service
CREATE TABLE Visit_Service (
    visit_id int  NOT NULL,
    service_id int  NOT NULL,
    service_fee decimal(10,2)  NOT NULL,
    CONSTRAINT Visit_Service_pk PRIMARY KEY  (service_id,visit_id)
);

-- foreign keys
-- Reference: Appointment_Doctor (table: Visit)
ALTER TABLE Visit ADD CONSTRAINT Appointment_Doctor
    FOREIGN KEY (mechanic_id)
    REFERENCES Mechanic (mechanic_id);

-- Reference: Appointment_Patient (table: Visit)
ALTER TABLE Visit ADD CONSTRAINT Appointment_Patient
    FOREIGN KEY (client_id)
    REFERENCES Client (client_id);

-- Reference: Appointment_Service_Appointment (table: Visit_Service)
ALTER TABLE Visit_Service ADD CONSTRAINT Appointment_Service_Appointment
    FOREIGN KEY (visit_id)
    REFERENCES Visit (visit_id);

-- Reference: Appointment_Service_Service (table: Visit_Service)
ALTER TABLE Visit_Service ADD CONSTRAINT Appointment_Service_Service
    FOREIGN KEY (service_id)
    REFERENCES Service (service_id);

-- End of file.

-- Insert Clients
INSERT INTO Client (client_id, first_name, last_name, date_of_birth) VALUES
(1, N'Adam', N'Kowal', '1980-02-15 00:00:00'),
(2, N'Monika', N'Maj', '1992-06-24 00:00:00'),
(3, N'Karol', N'Sobczak', '1987-11-05 00:00:00'),
(4, N'Natalia', N'Lis', '1975-12-30 00:00:00'),
(5, N'Bartosz', N'Król', '1999-09-18 00:00:00');

-- Insert Mechanics
INSERT INTO Mechanic (mechanic_id, first_name, last_name, licence_number) VALUES
(1, N'Łukasz', N'Nowak', N'MECH-20230001'),
(2, N'Paweł', N'Zieliński', N'MECH-20230002'),
(3, N'Kamil', N'Wójcik', N'MECH-20230003'),
(4, N'Mateusz', N'Kaczmarek', N'MECH-20230004');

-- Insert Services
INSERT INTO Service (service_id, name, base_fee) VALUES
(1, N'Oil Change', 120.00),
(2, N'Tire Replacement', 300.00),
(3, N'Engine Diagnostics', 150.00),
(4, N'Brake Repair', 250.00),
(5, N'Battery Check', 80.00);

-- Insert Visits
INSERT INTO Visit (visit_id, client_id, mechanic_id, date) VALUES
(1, 1, 1, '2024-04-10 09:00:00'),
(2, 2, 2, '2024-04-11 10:30:00'),
(3, 3, 3, '2024-04-12 13:45:00'),
(4, 4, 1, '2024-04-13 15:00:00'),
(5, 5, 4, '2024-04-14 08:15:00'),
(6, 1, 2, '2024-04-15 11:00:00');

-- Insert Visit Services
INSERT INTO Visit_Service (visit_id, service_id, service_fee) VALUES
(1, 1, 120.00),   -- Oil Change
(1, 5, 80.00),    -- Battery Check
(2, 3, 150.00),   -- Engine Diagnostics
(3, 2, 300.00),   -- Tire Replacement
(4, 4, 250.00),   -- Brake Repair
(5, 1, 130.00),   -- Oil Change (custom fee)
(6, 3, 155.00),   -- Engine Diagnostics (custom fee)
(6, 5, 85.00);    -- Battery Check