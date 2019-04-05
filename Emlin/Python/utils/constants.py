import os
from sys import platform as _platform


DATA_PATH = os.path.join(os.getenv('APPDATA'), "Emlin")

DEBUG_DATA_PATH = os.path.dirname(__file__)
no_outlier_file_path = os.path.join(DATA_PATH, "D_KeyboardData_O.csv")
inverse_data_file_path = os.path.join(DATA_PATH, "D_KeyboardData_O_Not.csv")


