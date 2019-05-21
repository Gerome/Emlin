from matplotlib import pyplot as plt
from sklearn.model_selection import cross_val_predict


def get_score(clf, all_x, all_y):
    return cross_val_predict(clf, all_x, all_y, cv=5, verbose=True)
