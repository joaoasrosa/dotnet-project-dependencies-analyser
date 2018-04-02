using System;

namespace DotnetProjectDependenciesAnalyser.Domain
{
    public class AnalyseDependencies
    {
        private readonly AnalyserServicesFactory _analyserServicesFactory;

        public AnalyseDependencies(
            AnalyserServicesFactory analyserServicesFactory)
        {
            _analyserServicesFactory = analyserServicesFactory;
        }
        
        public Report Execute(
            AnalyseDependenciesSettings settings)
        {
            if(settings == null)
                throw new SettingsAreNull("The analyse dependencies settings are null.");
            
            if (settings.SettingsAreValid() == false)
                throw new SettingsAreInvalid("The analyse dependencies settings are invalid.");

            var analyserService = _analyserServicesFactory.CreateService(settings);

            return analyserService.Analyse();
        }
    }
}