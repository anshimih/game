using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_1
{
    public class Plate : Cell
    {
        public bool IsActive { get; private set; } // Состояние активации плиты
        private string originalValue;

        public Plate(int x, int y) : base(x, y, "O", false, true)
        {
            IsActive = false;
            originalValue = "O"; // Сохраняем оригинальное значение
        }

        // Метод для активации плиты камнем
        public void ActivateByRock()
        {
            IsActive = true;
            Value = "@"; // Плита активирована камнем
        }

        // Метод для активации плиты персонажем
        public void ActivateByCharacter()
        {
            IsActive = true;
            Value = "₽"; // Плита активирована персонажем
        }

        // Метод для деактивации плиты
        public void Deactivate()
        {
            IsActive = false;
            Value = originalValue; // Восстанавливаем оригинальное значение
        }

        public override string ToString()
        {
            return IsActive ? Value : originalValue; // Отображаем активированное или оригинальное значение
        }
    }

}
