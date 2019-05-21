import numpy as np
import pandas as pd
from matplotlib import pyplot as plt
from utils.eval import get_score
from sklearn.metrics import precision_recall_curve, roc_curve

def plot_confusion_matrix(df_confusion, title='Confusion matrix', cmap=plt.cm.gray_r):
    plt.matshow(df_confusion, cmap=cmap) # imshow
    #plt.title(title)
    plt.colorbar()
    tick_marks = np.arange(len(df_confusion.columns))
    plt.xticks(tick_marks, df_confusion.columns, rotation=45)
    plt.yticks(tick_marks, df_confusion.index)
    #plt.tight_layout()
    plt.ylabel(df_confusion.index.name)
    plt.xlabel(df_confusion.columns.name)
    plt.show()


def show_confusion_matrix(clf, test_x, test_y):
    y_actu = pd.Series(test_y, name='Actual')
    y_pred = pd.Series(clf.predict(test_x), name='Predicted')
    df_confusion = pd.crosstab(y_actu, y_pred, margins=True)
    print(df_confusion)
    plot_confusion_matrix(df_confusion)


def show_precision_recall(clf, test_x, test_y):
    y_scores = get_score(clf, test_x, test_y)
    precisions, recalls, thresholds = precision_recall_curve(test_y, y_scores)
    print(precisions)
    print(recalls)
    print(thresholds)
    plt.plot(thresholds, precisions[:-1], "b--", label="Precision")
    plt.plot(thresholds, recalls[:-1], "g-", label="Recall")
    plt.xlabel("Threshold")
    plt.legend(loc="upper left")
    plt.show()


def show_roc_curve(clf, test_x, test_y):
    y_scores = get_score(clf, test_x, test_y)
    fpr, tpr, thresholds = roc_curve(test_y, y_scores)

    print(fpr)
    print(tpr)
    print(thresholds)

    plt.plot(fpr, tpr, label=None)
    plt.axis([0, 1, 0, 1])
    plt.xlabel('False Positive Rate')
    plt.ylabel('True Positive Rate')
    plt.show()


def plot_data(comb_id, all_x_data):
    visual_identifiers = ['bo', 'gx', 'r+', 'c+', 'mx', 'k^']

    title = get_graph_title(comb_id/6000)

    plt.title(title)
    plt.xlabel("Hold time")
    plt.ylabel("Flight time")

    user_idx = 0

    for user_data in all_x_data:
        hold_time = user_data[:, 1]
        flight_time_time = user_data[:, 2]

        plt.axes().set_aspect('equal', 'datalim')
        legend_val = plt.plot(hold_time, flight_time_time, visual_identifiers[user_idx], label='test')
        user_idx += 1

    plt.close()


def get_graph_title(comb_id):
    actual_comb_id = int(comb_id/6000)
    first_ascii = actual_comb_id // 128
    second_ascii = int(((actual_comb_id / 128) % 1) * 128)

    first_char_of_comb = get_ascii_replacement(first_ascii)
    second_char_of_comb = get_ascii_replacement(second_ascii)

    title = "\'" + first_char_of_comb + '\' to \'' + second_char_of_comb + "\'"
    return title


def get_ascii_replacement(second_ascii):

    switcher = {
        8: "BackSpace",
        9: "Hz-Tab",
        10: "Line-Feed",
        11: "Vertical-Feed",
        12: "Form-Feed",
        13: "Carriage-Return",
        14: "Shift-Out",
        15: "Shift-In",
        16: "Data-Link-Esc",
        17: "Device-Control-1",
        18: "Device-Control-2",
        19: "Device-Control-3",
        20: "Device-Control-4",
        21: "Negative-Acknowledge",
        22: "Synch-Idle",
        23: "End-of-Trans",
        24: "Cancel",
        25: "End-of-Medium",
        26: "Substitute",
        27: "Escape",
        28: "File-Separator",
        29: "Group-Separator",
        30: "Record-Separator",
        31: "Unit-Separator",
        32: "Space",
    }

    return switcher.get(second_ascii,  chr(second_ascii))



















