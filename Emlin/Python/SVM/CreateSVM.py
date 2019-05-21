# Other librarys
import os
import pandas as pd
import numpy as np
import sys

# User files
from sklearn.metrics import classification_report
from sklearn.model_selection import GridSearchCV
from sklearn.neural_network import MLPClassifier
from sklearn.preprocessing import StandardScaler
from sklearn.svm import SVC

import utils.eval
import utils.plot
from utils import model_pers as persistence
from SVM import SVMModel


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

    #param_grid = [{'hidden_layer_sizes': [(25, 25, ), (100, 100, )]}]

    scaler = StandardScaler()
    scaler.fit(X_train)
    X_train = scaler.transform(X_train)

    knn_clf = SVMModel.GetSVMClassifier()

    knn_clf.fit(X_train, y_train)

    test_data = load_test_data()

    X_test = test_data[['Id', 'HT', 'FT']].values
    X_test = scaler.transform(X_test)

    y_test = np.ravel(test_data[['User']].values)

    #grid_search = do_grid_search(X_train, param_grid, y_train, X_test, y_test)

    #utils.plot.show_confusion_matrix(knn_clf, X_test, y_test)
    #utils.plot.show_precision_recall(knn_clf, X_test, y_test)
    #utils.plot.show_roc_curve(knn_clf, X_test, y_test)
    persistence.save_model(knn_clf, DATA_PATH, "svmClf")


def do_grid_search(X_train, param_grid, y_train, X_test, y_test):
    grid_search = GridSearchCV(MLPClassifier(), param_grid)
    grid_search.fit(X_train, y_train)

    grid_search.fit(X_train, y_train)

    print("Best parameters set found on development set:")
    print()
    print(grid_search.best_params_)
    print()
    print("Grid scores on development set:")
    print()
    means = grid_search.cv_results_['mean_test_score']
    stds = grid_search.cv_results_['std_test_score']
    for mean, std, params in zip(means, stds, grid_search.cv_results_['params']):
        print("%0.3f (+/-%0.03f) for %r"
              % (mean, std * 2, params))
    print()

    print("Detailed classification report:")
    print()
    print("The model is trained on the full development set.")
    print("The scores are computed on the full evaluation set.")
    print()
    y_true, y_pred = y_test, grid_search.predict(X_test)
    print(classification_report(y_true, y_pred))
    print()

    return grid_search


if __name__ == "__main__":
    sys.exit(int(main() or 0))

