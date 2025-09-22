import express from "express";
import cors from "cors";
import sequelize from "./config/sequelize.js";
import moviesRouter from "./routes/movies.js";

const app = express();
app.use(
  cors({
    origin: "*",
    credentials: true,
  })
);
app.use(express.json());

app.use("/api/movies", moviesRouter);

sequelize
  .sync({ alter: true })
  .then(() => console.log("Database synchronized"))
  .catch((err) => console.error("DB sync error:", err));

const PORT = 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
