# Other librarys
import os
import pandas as pd
import numpy as np
import sys
from sklearn.model_selection import train_test_split

# User files
import ModelUtils as MU
import SVMModel


DATA_PATH = Constants.DATA_PATH

def load_group_data(data_path=DATA_PATH):
		csv_path = os.path.join(data_path, "GroupedData.csv")
		return pd.read_csv(csv_path)

def main():
	group_data = load_group_data()
	print(group_data.head())


	all_x = group_data[['Id', 'HT', 'FT', 'Di1', 'Di2', 'Di3']].values
	all_y = group_data[['User']].values
	all_y = np.ravel(all_y)

	X_train, X_test, y_train, y_test = train_test_split(all_x, all_y, test_size=0.2, random_state=42)

	svm_clf = SVMModel.GetSVMClassifier()
	#svm_scores = MU.GetScoreFrom(svm_clf, all_x, all_y)
	#print(svm_scores.mean())
	svm_clf.fit(X_train, y_train)
	MU.SaveModelAsJoblib(svm_clf, "svmClf")

if __name__ == "__main__":
    sys.exit(int(main() or 0))
