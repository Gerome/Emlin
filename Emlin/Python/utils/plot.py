import numpy as np
import pandas as pd
from matplotlib import pyplot as plt


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
    df_confusion = pd.crosstab(y_actu, y_pred)
    print(df_confusion)
    plot_confusion_matrix(df_confusion)


def plot_data(comb_id, all_x_data):
    visual_identifiers = ['ro', 'bx', 'g+', 'k+']

    title = get_graph_title(comb_id)

    plt.title(title)
    plt.xlabel("Flight time")
    plt.ylabel("Hold time")

    user_idx = 0

    for user_data in all_x_data:
        hold_time = user_data[:, 1]
        flight_time_time = user_data[:, 2]

        legend_val = plt.plot(hold_time, flight_time_time, visual_identifiers[user_idx], label='test')
        plt.legend([legend_val], ['Val'])
        user_idx += 1

    plt.close()


def get_graph_title(comb_id):
    actual_comb_id = int(comb_id)
    first_ascii = actual_comb_id // 128
    second_ascii = int(((actual_comb_id / 128) % 1) * 128)

    first_char_of_comb = get_ascii_replacement(first_ascii)
    second_char_of_comb = get_ascii_replacement(second_ascii)

    title = "\'" + first_char_of_comb + '\' to \'' + second_char_of_comb + "\'"
    return title


def get_ascii_replacement(second_ascii):
    if second_ascii == 8:
        second_char_of_comb = "BkSp"
    else:
        second_char_of_comb = chr(second_ascii)
    return second_char_of_comb