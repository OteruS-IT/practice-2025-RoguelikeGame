using UnityEngine;

namespace RogueSharpTutorial.View
{
    public class InputKeyboard : MonoBehaviour
    {
        [Header("Keybinds")]
        [SerializeField] KeyCode closeGameKey = KeyCode.Escape;
        [SerializeField] KeyCode interactionKey = KeyCode.E;
        [SerializeField] KeyCode rightKey = KeyCode.RightArrow;
        [SerializeField] KeyCode leftKey = KeyCode.LeftArrow;
        [SerializeField] KeyCode upKey = KeyCode.UpArrow;
        [SerializeField] KeyCode downKey = KeyCode.DownArrow;

        [Header("Repeat Settings")]
        [SerializeField] float initialDelay = 0.5f;  // задержка перед первым повтором
        [SerializeField] float repeatRate = 0.1f;    // интервал между повторами

        private InputCommands input = InputCommands.None;
        private InputCommands lastMoveDirection = InputCommands.None;
        private float nextMoveTime = 0f;

        public InputCommands Command
        {
            get
            {
                InputCommands result = input;
                input = InputCommands.None;
                return result;
            }
        }

        private void Update()
        {
            // Команды, которые не должны повторяться
            if (Input.GetKeyDown(interactionKey))
                input = InputCommands.StairsDown;

            if (Input.GetKeyDown(closeGameKey))
                input = InputCommands.CloseGame;

            // Определяем текущее зажатое направление
            InputCommands currentDirection = GetCurrentDirectionFromHeldKeys();

            // Логика повтора движения
            if (currentDirection != InputCommands.None)
            {
                // Если направление изменилось (включая переход от None)
                if (currentDirection != lastMoveDirection)
                {
                    // Сразу отправляем команду в новом направлении
                    input = currentDirection;
                    lastMoveDirection = currentDirection;
                    nextMoveTime = Time.time + initialDelay;
                }
                // Если направление не изменилось и пришло время повтора
                else if (Time.time >= nextMoveTime)
                {
                    input = currentDirection;
                    nextMoveTime = Time.time + repeatRate;
                }
            }
            else
            {
                // Клавиши отпущены – сбрасываем состояние
                lastMoveDirection = InputCommands.None;
                nextMoveTime = 0f;
            }
        }

        private InputCommands GetCurrentDirectionFromHeldKeys()
        {
            bool left = Input.GetKey(leftKey);
            bool right = Input.GetKey(rightKey);
            bool up = Input.GetKey(upKey);
            bool down = Input.GetKey(downKey);

            // Противоположные направления взаимно исключаются
            int horizontal = (right ? 1 : 0) - (left ? 1 : 0);
            int vertical = (up ? 1 : 0) - (down ? 1 : 0);

            if (horizontal > 0 && vertical > 0) return InputCommands.UpRight;
            if (horizontal > 0 && vertical < 0) return InputCommands.DownRight;
            if (horizontal < 0 && vertical > 0) return InputCommands.UpLeft;
            if (horizontal < 0 && vertical < 0) return InputCommands.DownLeft;
            if (horizontal > 0) return InputCommands.Right;
            if (horizontal < 0) return InputCommands.Left;
            if (vertical > 0) return InputCommands.Up;
            if (vertical < 0) return InputCommands.Down;

            return InputCommands.None;
        }
    }
}