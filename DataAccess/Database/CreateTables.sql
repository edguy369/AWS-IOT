CREATE TABLE Cabinets_Cabinet (
	uuid VARCHAR ( 36 ) PRIMARY KEY,
	mac_addr VARCHAR ( 100 ) UNIQUE NOT NULL,
	name VARCHAR ( 100 ) NOT NULL,
	gps_point VARCHAR ( 100 ) NOT NULL,
	created_on TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Cabinets_Slots_Status (
	id INTEGER PRIMARY KEY,
	name VARCHAR ( 100 ) NOT NULL
);

INSERT INTO Cabinets_Slots_Status (id, name) VALUES (1, 'Ready');
INSERT INTO Cabinets_Slots_Status (id, name) VALUES (2, 'In Use');
INSERT INTO Cabinets_Slots_Status (id, name) VALUES (3, 'Failure');

CREATE TABLE Cabinets_Slot (
	id INTEGER NOT NULL,
	status_id INTEGER NOT NULL REFERENCES Cabinets_Slots_Status (id),
	cabinet_uuid VARCHAR ( 36 ) NOT NULL REFERENCES Cabinets_Cabinet (uuid),
	last_update TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY(id, cabinet_uuid)
);

CREATE TABLE Cabinets_Last_Slot (
	cabinet_uuid VARCHAR ( 36 ) REFERENCES Cabinets_Cabinet (uuid),
	slot_opened INTEGER NOT NULL,
	PRIMARY KEY(cabinet_uuid)
);

CREATE TABLE Cabinets_Log (
	id SERIAL PRIMARY KEY,
	cabinet_uuid VARCHAR ( 36 ) REFERENCES Cabinets_Cabinet (uuid),
	user_uuid VARCHAR ( 36 ) NOT NULL,
	slot_id INTEGER NOT NULL,
	order_num VARCHAR ( 50 ) NOT NULL,
	start_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	end_time TIMESTAMP NULL 
);

CREATE TABLE Cabinets_Slot_Session (
	user_uuid VARCHAR ( 36 ) NOT NULL,
	cabinet_uuid VARCHAR ( 36 ) NOT NULL REFERENCES Cabinets_Cabinet (uuid),
	slot_id INTEGER NOT NULL,
	start_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY(user_uuid, cabinet_uuid, slot_id)
);

