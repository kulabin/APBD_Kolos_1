-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-05-06 20:25:35.992

-- foreign keys
ALTER TABLE Visit DROP CONSTRAINT Appointment_Doctor;

ALTER TABLE Visit DROP CONSTRAINT Appointment_Patient;

ALTER TABLE Visit_Service DROP CONSTRAINT Appointment_Service_Appointment;

ALTER TABLE Visit_Service DROP CONSTRAINT Appointment_Service_Service;

-- tables
DROP TABLE Client;

DROP TABLE Mechanic;

DROP TABLE Service;

DROP TABLE Visit;

DROP TABLE Visit_Service;

-- End of file.

