import { Router } from "express";
import { getMovies, addMovie, editMovie, removeMovie } from "../controllers/movies.js";

const router = Router();

router.get("/", getMovies);

router.post("/", addMovie);
router.put("/:id", editMovie);
router.delete("/:id", removeMovie);

export default router;
