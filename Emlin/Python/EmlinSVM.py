import os
import pandas as pd

import numpy as np
from sklearn.svm import SVC
from sklearn.model_selection import cross_val_score
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler
from sklearn.model_selection import train_test_split


DATA_PATH = "C:/Users/Gerome/Dropbox/CI301-The Individual Project/Data/Processed"

def load_group_data(data_path=DATA_PATH):
    csv_path = os.path.join(data_path, "GroupedData.csv")
    return pd.read_csv(csv_path)


group_data = load_group_data()
group_data.head()


all_x = group_data[['Id', 'HT', 'FT', 'Di1', 'Di2', 'Di3']].values
all_y = group_data[['User']].values
all_y = np.ravel(all_y)

svm_clf = Pipeline((
    ("scaler", StandardScaler()),
    ("linear_svc", SVC(kernel="poly", degree=2, coef0=1, C=5)),
))

svm_scores = cross_val_score(svm_clf, all_x, all_y, cv=5, verbose=True, n_jobs=5)

print(svm_scores.mean())

