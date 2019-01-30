# Other librarys
import os
import pandas as pd
import numpy as np
import sys
from sklearn.externals import joblib
from sklearn.neighbors import KNeighborsClassifier

# User files
import ModelUtils as MU

DATA_PATH = "C:/Users/Gerome/Dropbox/CI301-The Individual Project/Emlin/Data/Processed"

def load_group_data(data_path=DATA_PATH):
		csv_path = os.path.join(data_path, "GroupedData.csv")
		return pd.read_csv(csv_path)

def main():
	group_data = load_group_data()
	print(group_data.head())


	all_x = group_data[['Id', 'HT', 'FT', 'Di1', 'Di2', 'Di3']].values
	all_y = group_data[['User']].values
	all_y = np.ravel(all_y)

	numberOfNeighbours = 10
	for i in range(1, numberOfNeighbours):
		knn_clf = KNeighborsClassifier(n_neighbors=i)
	
		knn_scores = MU.GetScoreFromCLF(knn_clf, all_x, all_y)
		print("Evaluting " + i +" neighbours. Score is "+ knn_scores.mean())
		#knn_clf.fit(all_x, all_y)
		#MU.SaveModelAsJoblib(knn_clf, "knnClf")

if __name__ == "__main__":
    sys.exit(int(main() or 0))
