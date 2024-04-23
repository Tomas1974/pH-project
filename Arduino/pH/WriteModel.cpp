#include "WriteModel.h"
#include <Arduino.h>

WriteModel::WriteModel(String name, int value) : name1(name), value1(value) {}

String WriteModel::getName() {
    return name1;
}

int WriteModel::getValue() {
    return value1;
}

void WriteModel::setName(String name) {
    name1 = name;
}

void WriteModel::setValue(int value) {
    value1 = value;
}
