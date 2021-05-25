# "Сущности":

Основные:

Enum GameState{}

class Game:
  - Fields:
    - GameState gameState; //состояние игры
    - double actualTime; //хранит время с начала игры, для того, чтобы Action знали когда выполняться

class Action (хранит делегат действия игрока и время с начала игры, в которое он должен быть выполнен)
(название будет другим, во имя избежания конфликта, возможно проще хранить действия при помощи Tuple, возможно и по-другому, механизм хранения действий будет выбран во время разработки):
  - Fields:
    - Action action;
    - Double time;

class Explorer:
  - Fields:
    - Queue<Action> actions; //хранит действия совершенные игроком в определенный момент времени
    - Point actualCoordinates;
    - Point target; //точка, которую игрок должен достигнуть
    - bool copyFromPast; //определяет, чем управляется исследователь: вызываемыми из очереди действиями (если исследователь - копия из прошлого) или игроком с клавиатуры
  
  - Methods:
    - OnCollision() //исследователь не проходит сквозь стены
    - Run() //исследователь умеет бегать по твердому полу (сохраняется в actions)
    - Jump() //исследователь умеет прыгать (сохраняется в actions)
    - Rotate() //поворачивается влево и вправо (сохраняется в actions)
    - Stoop() //игрок нагибается для прохождения под низким потолком, сохраняется в actions) (сохраняется в actions)
    - ActLikeInThePast() //для копий - берет действие из очереди и выполняет его
    - GameOver() //для копий - считает расширяющийся треугольник от глаз копии до конца экрана (угол обзора), 
            при попадении игрока в треугольник (playerActualCoordinates) заканчинвает игр

class Block (прямоугольник, при помощи него можно рисовать стены и пол, сквозь которые не будет проходить персонаж):
  - Fields:
    - Point topLeft; //c bottomRight образуют прямоугольник, через который не может пройти игрок
    - Point bottomRigth;

Дополнительные, возможны изменения, удаления, добавление:
  
class Platform (наследуется от Block)
  - Fields:
    - Point extremeLeftPosition; //с extremeRightPosition задают точки, между которыми движется платформа
    - Point extremeRightPosition;
    - double speed;
  - Methods:
    - Move()

class Button
  - Fields:
    - Point position;
    - Action action; //действие по нажатию
    - bool Activated;

  - Methods:
    - OnCollision() //при контакте с игроком активируется, вызывая Action;

class Laser
  - Fields:
    - Point focusPoint; //точка, из которой выходит лазер
    - double direction; //напрвление, в радианах

  - Methods:
    - GameOver() //при пересечении персонажем линии луча он умирает

class TimeAnomaly
  - Fields:
    - Point position;
    - double radius;

  - Methods:
    - GameOver() //при попадении игрока в аномалию он умирает