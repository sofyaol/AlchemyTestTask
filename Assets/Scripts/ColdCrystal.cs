public class ColdCrystal : Crystal
{
    protected override void FillTemperatureLevels(int currentTemperature, int temperatureStep)
    {
        for (int i = 0; i < _levelCount; i++)
        {
            _temperatures[i] = currentTemperature;
            currentTemperature -= temperatureStep;
        }
    }

    protected override void SetOptimalTemperature(Ingredient ingredient, int temperatureLevel)
    {
        var currentTemperature = ingredient.Temperature;
        
        if (currentTemperature > _temperatures[temperatureLevel])
        {
            ingredient.Temperature = _temperatures[temperatureLevel];
            var temperatureStatus = ingredient.Temperature == 0 ? "cold" : "normal"; 
            _infoLabel.text = ingredient.name + " is " + temperatureStatus;
        }
    }
}
