import { DataTypes } from "sequelize";
import sequelize from "../config/sequelize.js";

const Movie = sequelize.define(
  "movies",
  {
    title: {
      type: DataTypes.STRING,
      allowNull: false,
    },
    genre: {
      type: DataTypes.STRING,
      allowNull: false,
    },
    release: {
      type: DataTypes.INTEGER,
      allowNull: false,
      validate: {
        isInt: true,
        len: [4],
      },
    },
    image_url: {
      type: DataTypes.STRING,
      allowNull: true,
      validate: {
        isUrl: true,
      },
    },
  },
  {
    timestamps: true,
  }
);

export default Movie;
