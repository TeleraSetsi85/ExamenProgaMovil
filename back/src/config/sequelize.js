import { Sequelize } from "sequelize";

const sequelize = new Sequelize("exam_progra", "root", "MySQLFT*13022005", {
  host: "localhost",
  dialect: "mysql",
  logging: false,
});

export default sequelize;
