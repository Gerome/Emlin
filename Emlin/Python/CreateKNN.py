# Other librarys
import os
import pandas as pd
import numpy as np
import sys

# User files
from sklearn.neighbors import KNeighborsClassifier

import utils.eval
import utils.plot
from utils import model_pers as persistence

#DATA_PATH = str(sys.argv[1])
DATA_PATH = "C:\\Users\\Gerome\\AppData\\Roaming\\Emlin"


def load_training_data(data_path=DATA_PATH):
    csv_path = os.path.join(data_path, "TrainData.csv")
    return pd.read_csv(csv_path)


def load_test_data(data_path=DATA_PATH):
    csv_path = os.path.join(data_path, "ValidationData.csv")
    return pd.read_csv(csv_path)


def main():
    train_data = load_training_data()

    X_train = train_data[['Id', 'HT', 'FT']].values
    y_train = train_data[['User']].values.astype(float)
    y_train = np.ravel(y_train)

    knn_clf = KNeighborsClassifier(1)

    knn_clf.fit(X_train, y_train)

    #test_data = load_test_data()

    #X_test = test_data[['Id', 'HT', 'FT']].values
    #y_test = np.ravel(test_data[['User']].values)

    #utils.plot.show_confusion_matrix(knn_clf, X_test, y_test)
    #utils.plot.show_precision_recall(knn_clf, X_test, y_test)
    #utils.plot.show_roc_curve(knn_clf, X_test, y_test)
    persistence.save_model(knn_clf, DATA_PATH, "knnClf")


if __name__ == "__main__":
    sys.exit(int(main() or 0))
