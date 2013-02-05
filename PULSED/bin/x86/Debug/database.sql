-- Table creation
CREATE TABLE users(id INTEGER PRIMARY KEY AUTOINCREMENT, username VARCHAR(255) NOT NULL);
CREATE TABLE questions(id INTEGER PRIMARY KEY AUTOINCREMENT, question TEXT NOT NULL);
CREATE TABLE answers(id INTEGER PRIMARY KEY AUTOINCREMENT,question_id INT NOT NULL, answer TEXT NOT NULL, FOREIGN KEY (question_id) REFERENCES questions(id));
CREATE TABLE correct_answer(question_id INT NOT NULL, answer_id INT NOT NULL, FOREIGN KEY (question_id) REFERENCES questions(id), FOREIGN KEY (answer_id) REFERENCES answers(id));
CREATE TABLE highscores(id INTEGER PRIMARY KEY AUTOINCREMENT, user_id INT NOT NULL, score INT NOT NULL, FOREIGN KEY (user_id) REFERENCES users(id));

-- Insert questions into the database
INSERT INTO questions (id, question) VALUES(0, 'Wie van deze personen loopt het grootste risico op een hartinfarct?');
INSERT INTO questions (id, question) VALUES(1, 'Wat is de correcte volgorde van handelen als je merkt dat iemand een hartstilstand heeft?');
INSERT INTO questions (id, question) VALUES(2, 'Hoeveel vingers gebruik je om een baby een hartmassage te geven?');
INSERT INTO questions (id, question) VALUES(3, 'Waar ligt meestal de oorzaak van een circulatiestilstand bij een baby?');
INSERT INTO questions (id, question) VALUES(4, 'Welk van deze gerechten kan je beter vermijden om gezonder te leven?');
INSERT INTO questions (id, question) VALUES(5, 'Word je dikker van dagelijks ontbijten en lunchen?');
INSERT INTO questions (id, question) VALUES(6, 'Waarom verhoogt overwicht het risico op een hartinfarct?');
INSERT INTO questions (id, question) VALUES(7, 'Waarom moet je dagelijks voldoende beweging hebben?');

-- Insert Answers into the database, connected with the question id
INSERT INTO answers(id, question_id, answer) VALUES(0,0,'Een 20 jarige student die dagelijks rookt');
INSERT INTO answers(id, question_id, answer) VALUES(1,0,'Een 30 jarige gymlerares waar de vader en moeder last van hart- en vaatziekten hebben.');
INSERT INTO answers(id, question_id, answer) VALUES(2,0,'Een 40 jarige luchtverkeersleider met een hoog cholesterol gehalte');
INSERT INTO answers(id, question_id, answer) VALUES(3,1,'Ambulance verwittigen, Reanimatie, Defribillatie');
INSERT INTO answers(id, question_id, answer) VALUES(4,1,'Reanimatie, Ambulance verwittigen, Defribillatie');
INSERT INTO answers(id, question_id, answer) VALUES(5,1,'Defribillatie, Ambulance verwittigen, Reanimatie');
INSERT INTO answers(id, question_id, answer) VALUES(6,2,'12');
INSERT INTO answers(id, question_id, answer) VALUES(7,2,'2');
INSERT INTO answers(id, question_id, answer) VALUES(8,2,'5');
INSERT INTO answers(id, question_id, answer) VALUES(9,3,'Zuurstoftekort');
INSERT INTO answers(id, question_id, answer) VALUES(10,3,'Blokkade van het hart');
INSERT INTO answers(id, question_id, answer) VALUES(11,3,'Te veel snoepjes');
INSERT INTO answers(id, question_id, answer) VALUES(12,4,'Sushi van een Sushi tent');
INSERT INTO answers(id, question_id, answer) VALUES(13,4,'Pizza van PizzaHut');
INSERT INTO answers(id, question_id, answer) VALUES(14,4,'Big Tasty van de McDonalds');
INSERT INTO answers(id, question_id, answer) VALUES(15,5,'Ja, je komt heel veel kilos aan hierdoor.');
INSERT INTO answers(id, question_id, answer) VALUES(16,5,'Nee, je valt juist sneller af!');
INSERT INTO answers(id, question_id, answer) VALUES(17,5,'Niet van lunchen, maar wel van ontbijten.');
INSERT INTO answers(id, question_id, answer) VALUES(18,6,'Je hart wordt zwaarder.');
INSERT INTO answers(id, question_id, answer) VALUES(19,6,'Je krijgt een verhoogde bloeddruk.');
INSERT INTO answers(id, question_id, answer) VALUES(20,6,'Doordat je dik wordt, krijgt je hart er geen zin meer in');
INSERT INTO answers(id, question_id, answer) VALUES(21,7,'Anders als je niet beweegt, beweeg je te weinig.');
INSERT INTO answers(id, question_id, answer) VALUES(22,7,'Je hebt meer kans op overgewicht, dat weer kan leiden tot een grotere kans op hart en vaatziekten.');
INSERT INTO answers(id, question_id, answer) VALUES(23,7,'Als je beweegt, krijgt slenderman je niet te pakken.');

-- Determine the correct answer for each question
INSERT INTO correct_answer VALUES(0,2);
INSERT INTO correct_answer VALUES(1,3);
INSERT INTO correct_answer VALUES(2,7);
INSERT INTO correct_answer VALUES(3,9);
INSERT INTO correct_answer VALUES(4,14);
INSERT INTO correct_answer VALUES(5,16);
INSERT INTO correct_answer VALUES(6,19);
INSERT INTO correct_answer VALUES(7,22);

