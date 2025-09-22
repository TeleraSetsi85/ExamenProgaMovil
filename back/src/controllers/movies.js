import Movie from "../models/movies.js";

export const getMovies = async (req, res) => {
  try {
    const movies = await Movie.findAll({
      order: [["createdAt", "DESC"]],
    });

    res.status(200).json(movies);
  } catch (error) {
    console.log(error);
    res.status(500).json({ error: "Error fetching movies" });
  }
};

export const addMovie = async (req, res) => {
  try {
    const { title, genre, release, image_url } = req.body;

    const newMovie = await Movie.create({
      title,
      genre,
      release,
      image_url,
    });

    res.status(201).json(newMovie);
  } catch (error) {
    console.log(error);
    res.status(500).json({ error: "Error adding movie" });
  }
};

export const editMovie = async (req, res) => {
  try {
    const { id } = req.params;
    const { title, genre, release, image_url } = req.body;

    const movie = await Movie.findByPk(id);
    if (!movie) return res.status(404).json({ error: "Movie not found" });

    await movie.update({ title, genre, release, image_url });

    res.status(200).json(movie);
  } catch (error) {
    console.log(error);
    res.status(500).json({ error: "Error editing movie" });
  }
};

export const removeMovie = async (req, res) => {
  try {
    const { id } = req.params;

    const movie = await Movie.findByPk(id);
    if (!movie) return res.status(404).json({ error: "Movie not found" });

    await movie.destroy();

    res.status(200).json({ message: "Movie deleted successfully" });
  } catch (error) {
    console.log(error);
    res.status(500).json({ error: "Error deleting movie" });
  }
};
