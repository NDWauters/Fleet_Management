import React from "react";
import { StyleSheet } from "react-native";

import colors from "../config/colors";
import AppText from "./AppText";

const ErrorMessage = ({ error, visible }) => {
  if (!visible || !error) return null;
  return <AppText style={styles.error}>{error}</AppText>;
};

export default ErrorMessage;

const styles = StyleSheet.create({
  error: {
    color: colors.redbright,
  },
});
