# Other librarys
import os
import pandas as pd
import numpy as np
import sys
from sklearn.model_selection import train_test_split
from random import randint

# User files
from Helper import model_pers as MU
from RFT import RFTModel
from utils import Constants

DATA_PATH = Constants.DATA_PATH

def load_group_data(data_path=DATA_PATH):
		csv_path = os.path.join(data_path, "GroupedData.csv")
		return pd.read_csv(csv_path)


def main():
    group_data = load_group_data()
    print(group_data.head())

    all_x = group_data[['Id', 'HT', 'FT']].values #'Di1', 'Di2', 'Di3']].values
    all_y = group_data[['User']].values
    all_y = np.ravel(all_y)



    all_x = all_x.astype(float)


    rft_clf = RFTModel.GetRFTClassifier()
    X_train, X_test, y_train, y_test = train_test_split(all_x, all_y, test_size=0.2, random_state=randint(0, 100))

    rft_clf.fit(all_x, all_y)
    print(rft_clf.feature_importances_)

    #y_scores = knn_clf.predict_proba(X_test)
    #fpr, tpr, threshold = roc_curve(y_test, y_scores[:, 1])
    #roc_auc = auc(fpr, tpr)

    #MU.ShowPrecisionRecall(fpr, tpr, roc_auc)
    #MU.ShowConfusionMatrix(knn_clf, X_test, y_test)
    MU.save_model(rft_clf, "rftClf")



if __name__ == "__main__":
    sys.exit(int(main() or 0))
