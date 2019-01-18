# Other librarys
import os
import pandas as pd
import numpy as np
import sys
from sklearn.model_selection import train_test_split
from sklearn.externals import joblib

# User files
import SVMModel


def main():
	DATA_PATH = "C:/Users/Gerome/Dropbox/CI301-The Individual Project/Data/Processed"

	def load_group_data(data_path=DATA_PATH):
		csv_path = os.path.join(data_path, "GroupedData.csv")
		return pd.read_csv(csv_path)


	group_data = load_group_data()
	print(group_data.head())


	all_x = group_data[['Id', 'HT', 'FT', 'Di1', 'Di2', 'Di3']].values
	all_y = group_data[['User']].values
	all_y = np.ravel(all_y)

	svm_clf = SVMModel.GetSVMClassifier();
	svm_scores = SVMModel.TrainSVM(svm_clf, all_x, all_y)

	print(svm_scores.mean())

if __name__ == "__main__":
    sys.exit(int(main() or 0))