# Other librarys
import os
import pandas as pd
import numpy as np
import sys
from sklearn.model_selection import train_test_split
from random import randint
from sklearn.metrics import average_precision_score

from sklearn.metrics import roc_curve
from sklearn.metrics import auc

# User files
import ModelUtils as MU
import KNNModel
import Constants


DATA_PATH = str(sys.argv[1])


def load_group_data(data_path=DATA_PATH):
    csv_path = os.path.join(data_path, "ProcessedData.csv")
    return pd.read_csv(csv_path)


printScores = False


def main():
    group_data = load_group_data()
    print(group_data.head())

    all_x = group_data[['Id', 'HT', 'FT']].values #'Di1', 'Di2', 'Di3']].values
    all_y = group_data[['User']].values
    all_y = np.ravel(all_y)

    all_x = all_x.astype(float)

    if printScores:
        printScoresOfNNeighbours(all_x, all_y)

    knn_clf = KNNModel.GetKNNClassifier(1)

    knn_clf.fit(all_x, all_y)

    #MU.ShowPrecisionRecall(fpr, tpr, roc_auc)
    #MU.ShowConfusionMatrix(knn_clf, X_test, y_test)
    MU.SaveModelAsJoblib(knn_clf, DATA_PATH, "knnClf")


def printScoresOfNNeighbours(all_x, all_y):
    numberOfNeighbours = 10
    for i in range(1, numberOfNeighbours):
        knn_clf = KNNModel.GetKNNClassifier(i)
        knn_scores = MU.GetScoreFromCLF(knn_clf, all_x, all_y)
        print(knn_scores)
        print("Evaluting " + str(i) + " neighbours. Score is " + str(knn_scores.mean()))


if __name__ == "__main__":
    sys.exit(int(main() or 0))
