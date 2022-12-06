class SpectrumInfo:
    def __init__(self, spectrumDict:dict):
        self.Name = spectrumDict['name']
        self.Color = spectrumDict['color']        
        self.LineStyle = spectrumDict['line_style']