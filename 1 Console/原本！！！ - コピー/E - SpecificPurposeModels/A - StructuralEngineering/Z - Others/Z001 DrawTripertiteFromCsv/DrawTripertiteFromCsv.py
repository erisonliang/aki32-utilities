# -*- coding:utf8 -*-
import json, os
from pathlib import Path
import pandas as pd
import matplotlib.pyplot as plt
from Class.SpectrumInfo import SpectrumInfo
from Class.Graph import Graph


### Initialize
BASE_PATH = '# TestModel'

OUTPUT_DIR_PATH = f'{BASE_PATH}/output'
INPUT_SPECTRUM_FILE_PATH = f'{BASE_PATH}/vel spectra.csv'
INPUT_CONFIG_DIR_PATH = f'{BASE_PATH}/configs'
INPUT_CONFIG_FILE_NAMES = [
  'All L1',
  'All L2',
]


### Define
def DrawTripertite(configFilePath, spectrumFilePath, saveImagePath):
    
    print(configName)
    print("  Started")
    
    ## Read config
    with open(configFilePath) as f:
        config = json.load(f)
        graph = Graph(config['graph'])
        specInfoList = [SpectrumInfo(spectrumDict) for spectrumDict in config['spectra']]

    ## Draw Tripertite
    for specInfo in specInfoList:
        df = pd.read_csv(spectrumFilePath)
        X = df[df.columns[0]].values
        Y = df[specInfo.Name].values
        graph.DrawVelSpectrumLine(X, Y, specInfo.Name, specInfo.Color, specInfo.LineStyle)

    graph.DrawPeriodLine()
    graph.Format()
    
    ## Save and show fig
    outputDir=Path(saveImagePath)
    if not os.path.exists(outputDir.parent):
        os.makedirs(outputDir.parent)
    plt.savefig(saveImagePath, dpi=graph.Dpi)
    print("  Done")


### Run
for configName in INPUT_CONFIG_FILE_NAMES:
    DrawTripertite(
        f'{INPUT_CONFIG_DIR_PATH}/{configName}.json',
        INPUT_SPECTRUM_FILE_PATH,
        f'{OUTPUT_DIR_PATH}/result {configName}.png')

print("All Done !")


