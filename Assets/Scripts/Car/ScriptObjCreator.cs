using System.Collections.Generic;
using UnityEngine;
using System.IO;

// UnityEngine.AssetDatabase является частью UnityEditor, поэтому этот скрипт будет работать только в редакторе
// Убедитесь, что этот скрипт не будет включен в сборку игры.
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScriptObjCreator : MonoBehaviour
{
    // Предполагаем, что у вас есть отдельный класс CarGoods, который наследуется от ScriptableObject
    // Например:
    // [CreateAssetMenu(fileName = "NewCarGoods", menuName = "Car/CarGoods")]
    // public class CarGoods : ScriptableObject { ... }

    public List<Car> _carPrefabs; // Список префабов машин

    // Добавьте поле для типа CarGoods, если оно отличается от того, что вы используете
    // public CarGoods carGoodsTemplate; // Опционально: шаблон для копирования

    private void Start()
    {
        foreach (Car carToProcess in _carPrefabs)
        {



            // 1. Создаем новый экземпляр Scriptable Object
            // Убедитесь, что класс CarGoods существует и наследуется от ScriptableObject
            CarGoods scriptableObjectInstance = ScriptableObject.CreateInstance<CarGoods>();

            // 2. Задаем данные для Scriptable Object
            scriptableObjectInstance.name = carToProcess.name; // Имя объекта в Project окне

            // --- Копируем или устанавливаем данные ---
            // В вашем коде было: scripObj._carPrefab = _carPrefabs[0];
            // Если CarGoods должен хранить ссылку на префаб Car, то так:
          //  scriptableObjectInstance._carPrefab = carToProcess; // !!! Важно: CarGoods должен иметь поле _carPrefab
          //  scriptableObjectInstance._name = carToProcess.name; // !!! Важно: CarGoods должен иметь поле _carPrefab

            // Если CarGoods имеет другие поля, которые нужно заполнить:
            // scriptableObjectInstance.someOtherField = carToProcess.someValue; // Пример

            // --- Определение пути и имени файла ---
            string folderPath = "Assets/Goods/CarGoods"; // Указываем папку, где должны быть .asset файлы
            string assetFileName = carToProcess.name + ".asset"; // Имя файла + расширение .asset
            string fullPath = Path.Combine(folderPath, assetFileName); // Соединяем путь и имя файла

            // --- Проверка наличия папки ---
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Debug.Log($"Создана папка: {folderPath}");
            }

            // --- Создание ассета ---
            // AssetDatabase.CreateAsset() ожидает полный путь к файлу
            AssetDatabase.CreateAsset(scriptableObjectInstance, fullPath);

            // --- Сохранение и обновление ---
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(); // Обновляем Project окно, чтобы увидеть новый ассет

            Debug.Log($"Scriptable Object '{scriptableObjectInstance.name}' создан и сохранен по пути: {fullPath}");
        }

    }
}